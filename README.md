# Login MFA Flow Application

# Diagrams

> Local Development Diagram

![Local Development](Flow/diagrams/Diagram.png)

---

> Application Security Diagram

![Application Security](Flow/diagrams/Application-Security-Diagram.png)

---

> SOC 2 TYPE 2 / OWASP 10 / ISO 27001 Security Compliance Diagram

![Security Compliance](Flow/diagrams/Security-Compliance-Diagram.png)

---

> CI/CD/GitOps Strategy Diagram

![Diagram GitOps](Flow/diagrams/GitOps-Diagram.png)

---
---

# Back-End Security

---

## SOC 2 TYPE 2 / OWASP 10 / ISO 27001 Compliance

---

### Security

> Protection of system resources and data against unauthorized access and disclosure.

#### Implementation: 

##### Secured Browsing

> Users Passwords Auth Encryption/Hashing: [BCrypt.Net package](https://www.nuget.org/packages/BCrypt.Net-Next)

> Users 2FA + QR Code Generation: [Otp.NET package](https://www.nuget.org/packages/Otp.NET/1.2.2)

> Apps CSRF Tokens Generation: [Blazor Antiforgery](https://learn.microsoft.com/en-us/aspnet/core/blazor/security/)

---

### Availability

> Accessibility of the system, products, or services as stipulated by service level agreements (SLAs).

#### Implementation: 

##### Secured Sandbox

> Sandboxed Environment: [Flow/docker](Flow/backend.Dockerfile)

> Sandboxed Environment Orchestration: [Flow/compose.yaml](Flow/compose.yaml)

---

### Processing Integrity

> Assurance that data processing is complete, valid, accurate, and timely.

#### Implementation: 

##### Uploaded Content

> Uploaded Content Isolation: [docker volumes](Flow/backend.Dockerfile) / [Flow/compose.yaml](Flow/compose.yaml)

> Uploaded Content Anti-Malware: [clamavnet](https://www.clamav.net/)

---

### Confidentiality

> Protection of sensitive information from unauthorized exposure.

#### Implementation: 

##### Data Traffic

> Database Data Anonymization: [System.Security.Cryptography package](https://www.nuget.org/packages/system.security.cryptography.pkcs/)

---

### Privacy

> Safeguarding personal information against unauthorized use or collection.

#### Implementation: 

##### Auditing & Tracing

> Tracing: [opentelemetry-dotnet package](https://opentelemetry.io/docs/languages/dotnet/)

---
---

# Code Structure

## CI/CD

> GitHub: [Flow/.github/workflows/github-actions.yml](Flow/.github/workflows/github-actions.yml)

> GitLab: [Flow/.gitlab-ci.yml](Flow/.gitlab-ci.yml)

> Jenkins: [Flow/Jenkinsfile](Flow/Jenkinsfile)

---

## GitOps

> Argo-CD Application Spec: [argo-cd-application-spec.yaml](Flow/.argo-cd/argo-cd-application-spec.yaml)

---

## DevSecOps

> Jenkins Container: [Flow/compose.yaml](Flow/compose.yaml) / [Flow/jenkins.Dockerfile](Flow/jenkins.Dockerfile)

> Jenkins Pipeline with Vulnerability Scanner, SBOM and SAST: [Flow/JenkinsfileScan](Flow/JenkinsfileScan)

> Docker Local Vulnerability Scanner, SBOM and SAST Container: [Flow/compose.yaml](Flow/compose.yaml) / [Flow/vulnerabilities.Dockerfile](Flow/vulnerabilities.Dockerfile)

> DAST Scanner Container and Config: [Flow/compose.yaml](Flow/compose.yaml)

- Vulnerability Scanner: [Trivy](https://github.com/aquasecurity/trivy)

- SBOM: [Syft](https://github.com/anchore/syft) / [Grype](https://github.com/anchore/grype)

- SAST: [Semgrep](https://github.com/semgrep/semgrep) / [Snyk](https://github.com/snyk/cli)

- DAST & Pen-Testing: [Nuclei](https://github.com/projectdiscovery/nuclei)

---

## SRE Monitoring

### Metrics

> Prometheus Config: [Flow/.prometheus/config/prometheus.yml](Flow/.prometheus/config/prometheus.yml)

> Prometheus Rules: [Flow/.prometheus/rules/prometheus.rules](Flow/.prometheus/rules/prometheus.rules)

> Prometheus Container: [Flow/compose.yaml](Flow/compose.yaml)

### Logging

> Loki Config (via Alloy): [Flow/.loki/config/loki-config.yaml](Flow/.loki/config/loki-config.yaml)

> Loki Container: [Flow/compose.yaml](Flow/compose.yaml)

### Tracing

> Tempo Config (via Alloy): [Flow/.tempo/config/tempo.yaml](Flow/.tempo/config/tempo.yaml)

> Tempo Container: [Flow/compose.yaml](Flow/compose.yaml)

### Resources and Networking

> OpenTelemetry Config: [opentelemetry-dotnet package](https://opentelemetry.io/docs/languages/dotnet/)

### Visualization

> Grafana Prometheus Datasource: [Flow/.grafana/datasources/prometheus-datasource.yaml](Flow/.grafana/datasources/prometheus-datasource.yaml)

> Grafana Loki Datasource: [Flow/.grafana/datasources/loki-datasource.yaml](Flow/.grafana/datasources/loki-datasource.yaml)

> Grafana Tempo Datasource: [Flow/.grafana/datasources/tempo-datasource.yaml](Flow/.grafana/datasources/tempo-datasource.yaml)

> Grafana Alert: [Flow/.grafana/alerting/sample-aspnet-alert-resource.yaml](Flow/.grafana/alerting/sample-aspnet-alert-resource.yaml)

> Grafana Container: [Flow/compose.yaml](Flow/compose.yaml)

### Alerting

> Alertmanager Config: [Flow/.alertmanager/config/alertmanager.yml](Flow/.alertmanager/config/alertmanager.yml)

> Alertmanager Container: [Flow/compose.yaml](Flow/compose.yaml)

### Unified Telemetry Collector

> Alloy Config (for Loki / Tempo): [Flow/.alloy/config/config.alloy](Flow/.alloy/config/config.alloy)

> Alloy Container: [Flow/compose.yaml](Flow/compose.yaml)

---

## Backend Execution

1. In terminal (Without Prometheus and Grafana stack):
```bash
dotnet watch
```

2. Orchestration with Docker Compose (With Prometheus and Grafana stack):
```bash
docker compose up --build --no-deps --force-recreate --remove-orphans
```

---

## IaC Config Tooling

> Ansible Inventory: [Flow/.ansible/inventory/docker_hosts.ini](Flow/.ansible/inventory/docker_hosts.ini)

> Ansible Vulnerabilities Playbook: [Flow/.ansible/playbooks/vulnerabilities_local_scan.yaml](Flow/.ansible/playbooks/vulnerabilities_local_scan.yaml)

> Ansible Host Dockerfile: [Flow/vulnerabilities.Dockerfile](Flow/vulnerabilities.Dockerfile)

> Ansible Python3.12+ Requirements: [Flow/ansible/ansible-requirements.txt](Flow/.ansible/ansible-requirements.txt)

```bash
python3 -m venv ./Flow/.ansible/.venv-ansible

source ./Flow/.ansible/.venv-ansible/bin/activate

python3 -m pip install -r ./Flow/.ansible/ansible-requirements.txt

ansible-inventory -i ./Flow/.ansible/inventory/docker_hosts.ini --list

ansible-playbook -i ./Flow/.ansible/inventory/docker_hosts.ini ./Flow/.ansible/playbooks/vulnerabilities_local_scan.yaml

deactivate

rm -rf ./Flow/.ansible/.venv-ansible
```
