# Snar Banking

[![SnarBanking API](https://github.com/lhargil/snarbanking/actions/workflows/snarbanking-api.yml/badge.svg)](https://github.com/lhargil/snarbanking/actions/workflows/snarbanking-api.yml)

Exploring ASP.NET Core ideas.

- Logging
- CQRS
- Dependency injection
- Problem details
- Testing
  - Integration
  - Unit
  - API
- Structure logging
- MongoDB

## Getting started

Pre-requisite

- [x] Docker desktop installed.
- [x] MongoDb Compass

Run the command below to get started. This creates a mongo db with seed data, a seq instance ingesting logs and a .NET 7 API using Endpoint routing.

```bash
$ docker compose up -d
```

```bash
[+] Running 3/3
 ✔ Container snarbanking-snarbankingDb-1    Started                        0.5s
 ✔ Container snarbanking-snarbankingSeq-1   Started                        0.5s
 ✔ Container snarbanking-snarbanking.api-1  Started                        1.1s
```

### Dashboards

Use [MongoDb Compass](https://www.mongodb.com/products/compass) to manage the seed data in the newly-created mongo db.

Login to `http://localhost:8080` to login to Seq to see the structure logs coming from the api. Use the following credentials

```
username: admin
password: <password>
```

For some reason, the pre-defined DEV password, `P@ssw0rd` doesn't seem to work. If that's the case, change the password once inside the Seq and navigate to `Settings > Users` page to update the password accordingly.
