# 🚀 Rover Mission Planner - API (.NET 8)

API REST desarrollada en ASP.NET Core (.NET 8) para gestionar tareas de exploración de rovers en un entorno simulado.

---

## ▶️ Cómo ejecutar la API

1. Clona el repositorio:

```bash
git clone https://github.com/Benzah1/RoverMissionPlanner.git
```

2. Entra en la carpeta del proyecto y ejecuta la API:

```bash
dotnet run --project API
```

3. Accede a Swagger para probar los endpoints:

```
http://localhost:5191/swagger
```

---

## 📊 Métricas con Prometheus

Las métricas están disponibles en:

```
http://localhost:5191/metrics
```

Puedes monitorearlas usando [Prometheus](https://prometheus.io/) y [Grafana](https://grafana.com/).

Ejemplo de configuración para Prometheus (`prometheus.yml`):

```yaml
global:
  scrape_interval: 5s

scrape_configs:
  - job_name: 'rover-api'
    static_configs:
      - targets: ['host.docker.internal:5191']
```

---

## ✅ Ejecutar pruebas unitarias

Para correr los tests:

```bash
dotnet test
```

---

## 📁 Estructura del Proyecto

```
RoverMissionPlanner/
├── API/                 # API ASP.NET Core
├── Application/         # Lógica de negocio
├── Domain/              # Entidades y enums
├── Infrastructure/      # Persistencia / Servicios internos
├── Application.Tests/   # Pruebas unitarias
└── README.md
```

---

## 🧪 Endpoints principales

- `POST /rovers/{roverName}/tasks` → Crear tarea
- `GET /rovers/{roverName}/tasks?date=YYYY-MM-DD` → Obtener tareas por día
- `GET /rovers/{roverName}/utilization?date=YYYY-MM-DD` → Obtener porcentaje de utilización

---

## 🛠️ Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Prometheus y/o Grafana (opcional)