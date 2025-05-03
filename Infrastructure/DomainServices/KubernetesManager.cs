using Domain.Dtos.Kubernetes;
using Domain.Dtos.Project;
using Domain.Entities;
using Domain.Enums;
using Domain.Utilities;
using k8s;
using k8s.KubeConfigModels;
using k8s.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;

namespace Infrastructure.DomainServices
{
    public class KubernetesManager
    {
        private readonly Kubernetes _k8sClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public KubernetesManager(Kubernetes k8sClient, IHttpContextAccessor httpContextAccessor)
        {
            _k8sClient = k8sClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task EnsureUserPodExistsAsync(KubernetesServiceDto service, V1Container container)
        {
            var existingPods = await _k8sClient.ListNamespacedPodAsync(service.Namespace, labelSelector: $"app={service.PodName}");
            if (existingPods.Items.Count > 0)
            {
                return;
            }

            var pvc = new V1PersistentVolumeClaim
            {
                ApiVersion = "v1",
                Kind = "PersistentVolumeClaim",
                Metadata = new V1ObjectMeta
                {
                    Name = service.PvcName,
                    NamespaceProperty = service.Namespace
                },
                Spec = new V1PersistentVolumeClaimSpec
                {
                    AccessModes = new List<string> { "ReadWriteOnce" },
                    Resources = new V1VolumeResourceRequirements
                    {
                        Requests = new Dictionary<string, ResourceQuantity>
                        {
                            { "storage", new ResourceQuantity(service.StorageSize) }
                        }
                    }
                }
            };

            await _k8sClient.CreateNamespacedPersistentVolumeClaimAsync(pvc, service.Namespace);

            var pod = new V1Pod
            {
                ApiVersion = "v1",
                Kind = "Pod",
                Metadata = new V1ObjectMeta
                {
                    Name = service.PodName,
                    NamespaceProperty = service.Namespace,
                    Labels = new Dictionary<string, string> { { "app", service.PodName }, { "serviceType", service.ServiceType.ToString().ToLower() } }
                },
                Spec = new V1PodSpec
                {
                    Containers = new List<V1Container> { container },
                    Volumes = new List<V1Volume>
                    {
                        new V1Volume
                        {
                            Name = service.VolumeName,
                            PersistentVolumeClaim = new V1PersistentVolumeClaimVolumeSource
                            {
                                ClaimName = $"{service.PodName}-pvc"
                            }
                        }
                    }
                }
            };

            await _k8sClient.CreateNamespacedPodAsync(pod, service.Namespace);
        }

        public async Task<NamespaceDto> CreateNamespaceAsync(string userId)
        {
            string namespaceName = $"{userId}-namespace";

            // Verifica se o namespace já existe
            var existingNamespace = await _k8sClient.ListNamespaceAsync();

            if (existingNamespace.Items.Any(ns => ns.Metadata.Name == namespaceName))
            {
                return new NamespaceDto(namespaceName);
            }

            // Cria um novo namespace
            var newNamespace = new V1Namespace
            {
                Metadata = new V1ObjectMeta { Name = namespaceName }
            };

            await _k8sClient.CreateNamespaceAsync(newNamespace);

            return new NamespaceDto(namespaceName);
        }

        public async Task CreateServiceAsync(KubernetesServiceDto serviceDto, V1Container container)
        {
            var service = new V1Service
            {
                ApiVersion = "v1",
                Kind = "Service",
                Metadata = new V1ObjectMeta
                {
                    Name = serviceDto.ServiceName,
                    NamespaceProperty = serviceDto.Namespace
                },
                Spec = new V1ServiceSpec
                {
                    Selector = new Dictionary<string, string> { { "app", serviceDto.PodName } },
                    Ports = new List<V1ServicePort>()
                }
            };

            foreach (var containerPort in container.Ports)
            {
                service.Spec.Ports.Add(new V1ServicePort
                {
                    Port = containerPort.ContainerPort,
                    TargetPort = containerPort.ContainerPort,
                    Protocol = "TCP",
                    Name = $"port-{containerPort.ContainerPort}"
                });
            }

            service.Spec.Type = "LoadBalancer";

            await _k8sClient.CreateNamespacedServiceAsync(service, serviceDto.Namespace);
        }

        public async Task<string> CreateServiceIngressAsync(KubernetesServiceDto service, V1Container container)
        {

            // Gera um host único para o Ingress
            string ingressHost = $"{service.PodName.Replace(" ", "-")}.{StringHelper.GenerateRandomStringFromGuid()}.{_httpContextAccessor.HttpContext.Request.Host.Host}";

            // Cria o objeto Ingress
            var ingress = new V1Ingress
            {
                ApiVersion = "networking.k8s.io/v1",
                Kind = "Ingress",
                Metadata = new V1ObjectMeta
                {
                    Name = $"{service.PodName}-ingress",
                    NamespaceProperty = service.Namespace,
                    Annotations = new Dictionary<string, string>
            {
                // Anotação para usar o Ingress Nginx
                { "nginx.ingress.kubernetes.io/backend-protocol", "TCP" }
            }
                },
                Spec = new V1IngressSpec
                {
                    // Define o IngressClass como "nginx" (padrão do Nginx Ingress Controller)
                    IngressClassName = "nginx",
                    Rules = new List<V1IngressRule>()
                }
            };

            // Adiciona regras para cada porta do container
            foreach (var containerPort in container.Ports)
            {
                ingress.Spec.Rules.Add(
                    new V1IngressRule
                    {
                        Host = ingressHost, // Subdomínio único para o usuário
                        Http = new V1HTTPIngressRuleValue
                        {
                            Paths = new List<V1HTTPIngressPath>
                            {
                                new V1HTTPIngressPath
                                {
                                    Path = "/", // Rota padrão
                                    PathType = "Prefix",
                                    Backend = new V1IngressBackend
                                    {
                                        Service = new V1IngressServiceBackend
                                        {
                                            Name = service.ServiceName, // Nome do serviço
                                            Port = new V1ServiceBackendPort
                                            {
                                                Number = containerPort.ContainerPort // Porta do serviço
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                );
            }

            // Cria o Ingress no Kubernetes
            await _k8sClient.CreateNamespacedIngressAsync(ingress, service.Namespace);

            // Retorna o endereço do serviço (subdomínio + porta)
            return $"{ingressHost}:{container.Ports.FirstOrDefault().ContainerPort}";
        }

        public async Task DeletePodAsync(KubernetesServiceDto service)
        {
            string ingressName = $"{service.PodName}-ingress";

            await _k8sClient.DeleteNamespacedServiceAsync(service.ServiceName, service.Namespace);
            await _k8sClient.DeleteNamespacedIngressAsync(ingressName, service.Namespace);
            await _k8sClient.DeleteNamespacedPodAsync(service.PodName, service.Namespace);
            await _k8sClient.DeleteCollectionNamespacedPersistentVolumeClaimAsync(service.Namespace);
        }

        public async Task<ContainerStatusType> GetContainerStatusAsync(KubernetesServiceDto service)
        {
            var existingPods = await _k8sClient.ListNamespacedPodAsync(service.Namespace, labelSelector: $"app={service.PodName}");

            if (existingPods.Items.Count == 0)
            {
                return ContainerStatusType.PodNotFound;
            }

            var pod = existingPods.Items.First();

            // Verifica o status do container
            var containerStatus = pod.Status?.ContainerStatuses?.FirstOrDefault(c => c.Name == service.ServiceType.ToString().ToLower());

            if (containerStatus == null)
            {
                return ContainerStatusType.ContainerNotFound;
            }

            if (containerStatus.State?.Running != null)
            {
                return ContainerStatusType.Running;
            }

            if (containerStatus.State?.Terminated != null)
            {
                return ContainerStatusType.Terminated;
            }

            if (containerStatus.State?.Waiting != null)
            {
                return ContainerStatusType.Waiting;
            }

            return ContainerStatusType.Unknown;
        }

        public async Task<Domain.Dtos.Kubernetes.ContainerMetrics> GetContainerMetricsAsync(KubernetesServiceDto service)
        {
            var metrics = new Domain.Dtos.Kubernetes.ContainerMetrics() { Usage = new() };

            metrics.UpTime = await GetPodUptimeAsync(service);

            try
            {
                var pod = await _k8sClient.ReadNamespacedPodAsync(service.PodName, service.Namespace);

                var containers = pod.Spec.Containers;

                foreach (var container in containers)
                {
                    if (container.Resources != null)
                    {

                        if (container.Resources.Requests.ContainsKey("cpu"))
                        {
                            metrics.Usage.Cpu = StringHelper.GetBytesFromSize(container.Resources.Requests["cpu"].ToString());
                        }

                        if (container.Resources.Requests.ContainsKey("memory"))
                        {
                            metrics.Usage.Memory = StringHelper.GetBytesFromSize(container.Resources.Requests["memory"].ToString());
                        }
                    }
                }

                return metrics;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter recursos de pod: {ex.Message}");
                return metrics;
            }
        }

        public async Task<double> GetPodUptimeAsync(KubernetesServiceDto service)
        {
            // Obter as informações do pod
            var pod = await _k8sClient.ReadNamespacedPodAsync(service.PodName, service.Namespace);

            // Obter a data de criação do pod
            var creationTime = pod.Metadata.CreationTimestamp;

            // Calcular a diferença de tempo entre agora e a criação do pod
            var uptime = DateTime.UtcNow - creationTime;

            return uptime.Value.TotalHours;
        }

    }
}