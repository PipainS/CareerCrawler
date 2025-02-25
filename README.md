<!-- Logo Section -->
<div align="center">
  <img src="CareerCrawler/assets/logo/career-crawler-logo.png" alt="CareerCrawler Logo" width="300">
  <h1>CareerCrawler</h1>
</div>

<!-- Status Badges -->
<div align="center">

[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-512BD4?logo=.net&style=flat-square)](https://dotnet.microsoft.com)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg?style=flat-square)](LICENSE)
[![Last Commit](https://img.shields.io/github/last-commit/PipainS/HHParser?color=green&logo=git&style=flat-square)](https://github.com/PipainS/HHParser/commits/main)
[![Repo Size](https://img.shields.io/github/repo-size/PipainS/HHParser?color=informational&style=flat-square)](https://github.com/PipainS/HHParser)

</div>

---

## 📖 About

**CareerCrawler** is a powerful console application for analyzing job market data through API aggregation. Currently supporting HH.ru with plans to expand to other platforms.

**Key Features**:
- 🎯 Professional role/specialization catalogs
- 💾 Vacancy dataset generation (C# example: 5000+ entries)
- 📁 CSV export capabilities
- 🔁 Resilient API communication with Polly retries

🔮 **Future Roadmap**
```mermaid
graph LR
  A[Current: HH.ru] --> B[Rabota.ru Integration]
  B --> C[LinkedIn Jobs API]
  C --> D[Salary Analytics Dashboard]
  D --> E[Automated Resume Analysis]
```
## 🚀 Getting Started

To run the project locally, follow these steps:

### Prerequisites
- [.NET 8.0](https://dotnet.microsoft.com/) must be installed.

### Setup Instructions

```bash
# Clone repository
git clone https://github.com/PipainS/CareerCrawler.git
cd CareerCrawler/CareerCrawler

# Restore packages
dotnet restore

# Run with sample parameters
dotnet run
```
## 📜 License

```text
MIT License

Copyright (c) 2024 PipainS
```
<div align="center" style="margin-top: 40px"> <img src="https://img.shields.io/badge/Made%20with-♥%20in%20C%23-blue?style=for-the-badge"> </div> 
