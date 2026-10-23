# Semesteropgave: Full-Stack Webapplikation med .NET Aspire, Web API og Blazor WASM

## 1. Introduktion og Formål
Formålet med denne opgave er at designe og implementere en sammenhængende, cloud-ready full-stack webapplikation. I skal demonstrere evnen til at strukturere et moderne .NET-system opdelt i uafhængige komponenter (frontend, backend og database), orkestreret og overvåget via **.NET Aspire**.

I skal vælge **én** af følgende to cases:
* **Valgmulighed A:** *SubTrack* – Personlig Abonnements- & Udgiftsstyring
* **Valgmulighed B:** *FitPulse* – Trænings- og Øvelseslogbog

---

## 2. Arkitektur og Teknologistak

Løsningen skal bestå af følgende delprojekter i jeres solution:

```
[ Aspire AppHost ] (Orkestrering & Ressourcestyring)
       │
       ├──► [ PostgreSQL Server (Container) ] ◄──► [ pgAdmin (Web GUI) ]
       │            ▲
       │            │ Forbindelsesstreng (Service Discovery)
       │            │
       ├──► [ ASP.NET Core Web API ] (Controller-baseret backend + EF Core)
       │            ▲
       │            │ HTTP / JSON (Service Discovery)
       │            │
       └──► [ Blazor WebAssembly ] (Single Page Application frontend)
```

* **Frontend:** Blazor WebAssembly (.NET)
* **Backend:** ASP.NET Core Web API (**Controller-baseret**)
* **Database:** PostgreSQL (kører som container styret af Aspire)
* **Datadistribution:** Entity Framework Core (`Npgsql.EntityFrameworkCore.PostgreSQL`)
* **Orkestrering & Telemetri:** .NET Aspire (`Aspire.AppHost` og `Aspire.ServiceDefaults`)

---

## 3. Aspire AppHost & PostgreSQL Konfiguration

I `AspireApp.AppHost/AppHost.cs` er PostgreSQL, data-persistens og pgAdmin defineret således:

```csharp
var builder = DistributedApplication.CreateBuilder(args);

// 1. Opretter PostgreSQL containerressource
var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin();

// 2. Opretter logisk database inde i serveren
var db = postgres.AddDatabase("appdb");

builder.Build().Run();
```

### Hvad gør disse kodelinjer?

1. **`builder.AddPostgres("postgres")`**:
   * Instruerer Aspire i at hente og starte et officielt PostgreSQL container-image (via Docker Desktop / Podman). Containeren tildeles navnet `"postgres"` i Aspire Dashboard.
2. **`.WithDataVolume()`**:
   * Etablerer et Docker *named volume* på jeres værtsmaskine, som monteres i PostgreSQL-containerens datamappe (`/var/lib/postgresql/data`).
   * **Hvorfor det er vigtigt:** Uden dette vil alle data og tabeller gå tabt, hver gang I stopper debug-sessionen eller genstarter maskinen. Med `WithDataVolume()` overlever jeres data genstart af containere.
3. **`.WithPgAdmin()`**:
   * Starter en separat hjælpe-container med værktøjet **pgAdmin** (et webbaseret administrationsinterface til PostgreSQL).
   * Aspire konfigurerer automatisk forbindelsen mellem pgAdmin og jeres PostgreSQL-instans. I Aspire Dashboard får I et direkte link til pgAdmin, hvor I kan logge ind, inspicere tabeller, køre rå SQL-queries og fejlfinde data.
4. **`postgres.AddDatabase("appdb")`**:
   * Definerer en specifik database (`appdb`) inde i PostgreSQL-serveren. Denne variabel bruges senere til at injicere forbindelsesoplysninger i jeres Web API via `.WithReference(db)`.

---

## 4. Case-valg (Vælg én)

---

### Case A: SubTrack – Personlig Abonnements- & Udgiftsstyring

#### Formål & Koncept
Mange forbrugere mister overblikket over deres faste digitale abonnementer (streaming, fitness, software, aviser, madkasser osv.). *SubTrack* skal give brugeren fuldt overblik over aktive abonnementer, kommende betalinger og det samlede månedlige/årlige forbrug.

#### Funktionelle Krav
1. **Håndtering af Abonnementer (CRUD):**
   * Oprettelse, visning, redigering og sletning/opsigelse af abonnementer.
   * Hvert abonnement skal indeholde: Navn (f.eks. "Netflix"), Kategori, Pris, Valuta (standard DKK), Betalingsinterval (Månedlig, Kvartalsvis, Årlig), Startdato, Næste fornyelsesdato samt Status (Aktiv, Pauset, Opsagt).
2. **Kategorier & Betalingsmidler:**
   * Mulighed for at knytte abonnementer til foruddefinerede eller brugeroprettede kategorier (f.eks. "Underholdning", "Fitness", "Produktivitet").
   * Mulighed for angivelse af betalingsmetode (f.eks. "Visa/Dankort", "MobilePay", "PayPal").
3. **Økonomisk Nøglebordsberegning (Dashboard / Aggregering):**
   * Beregning af normaliseret månedlig omkostning (et årligt abonnement på 1.200 kr. tæller som 100 kr./md.).
   * Beregning af forventet samlet årsforbrug.
   * Fordeling af udgifter grupperet pr. kategori (til visualisering).
4. **Notifikation/Advarsel om fornyelse:**
   * Fremhævning af abonnementer, der fornyes inden for de næste 7 dage, så brugeren kan nå at reagere/opsige.

#### Datamodel (Vejledende)
* `Subscription` (Id, Name, Price, BillingCycle, NextBillingDate, Status, CategoryId, PaymentMethodId)
* `Category` (Id, Name, ColorCode/Icon)
* `PaymentMethod` (Id, Name)

---

### Case B: FitPulse – Trænings- og Øvelseslogbog

#### Formål & Koncept
*FitPulse* er en træningsapplikation designet til styrketrænende personer, der ønsker at logge deres træningspas med øvelser, sæt, vægt og gentagelser (reps), samt følge udviklingen over tid.

#### Funktionelle Krav
1. **Øvelseskatalog (Master Data):**
   * Katalog over standardøvelser (f.eks. "Bænkpres", "Squat", "Dødløft", "Skulderpres").
   * Hver øvelse tilhører en muskelgruppe (f.eks. Bryst, Ben, Ryg, Skuldre) og en kategori (f.eks. Frivægte, Maskine, Kropsvægt).
   * Mulighed for at søge og filtrere i øvelser.
2. **Registrering af Træningspas (Kompleks transaktion):**
   * Brugeren kan oprette et nyt træningspas (`WorkoutSession`) med dato, tidspunkt, varighed og eventuelle noter.
   * Under samme træningspas tilføjes en eller flere øvelser (`WorkoutExercise`).
   * For hver øvelse logges en række sæt (`ExerciseSet`) med sæt-nummer, vægt i kg, antal reps og eventuelt RPE/notat.
3. **Historik & Detaljevisning:**
   * Kronologisk oversigt over gennemførte træningspas.
   * Detaljevisning af et specifikt pas med visning af alle øvelser og tilhørende sæt.
   * Sletning eller redigering af tidligere loggede sessioner.
4. **Statistik / Progress-beregning:**
   * Beregning af total volumen (Vægt × Reps summeret over alle sæt) for et træningspas.
   * Visning af personlig rekord (højeste vægt eller højeste volumen for en valgt øvelse).

#### Datamodel (Vejledende)
* `Exercise` (Id, Name, MuscleGroup, EquipmentType)
* `WorkoutSession` (Id, Date, DurationMinutes, Notes)
* `WorkoutExercise` (Id, WorkoutSessionId, ExerciseId, OrderIndex)
* `ExerciseSet` (Id, WorkoutExerciseId, SetNumber, WeightKg, Reps)

---

## 5. Tekniske Krav (Gælder for begge cases)

### A. ASP.NET Core Web API (Backend)
1. **Controller-baseret arkitektur:**
   * API'et skal implementeres ved brug af controllers arvet fra `ControllerBase` og dekoreret med `[ApiController]` og `[Route("api/[controller]")]`. Minimal APIs må **ikke** benyttes til denne opgave.
2. **RESTful Design & HTTP Statuskoder:**
   * Standardiserede HTTP-verber (`GET`, `POST`, `PUT`, `DELETE`).
   * Korrekt brug af statuskoder: `200 OK`, `201 Created` (med `CreatedAtAction` og `Location` header ved oprettelser), `204 NoContent` ved sletning, `400 BadRequest` ved valideringsfejl og `404 NotFound` når ressourcen ikke eksisterer.
3. **DTO Mønster (Data Transfer Objects):**
   * Entity Framework entity-klasser må **aldrig** sendes direkte ud af eller ind i controllerne over HTTP.
   * Der skal anvendes særskilte DTO-klasser til Requests og Responses (f.eks. `CreateSubscriptionRequestDto`, `SubscriptionResponseDto`, `WorkoutSummaryDto`).
4. **Validering:**
   * DTO'er skal valideres via Data Annotations (f.eks. `[Required]`, `[Range]`, `[StringLength]`).
5. **Entity Framework Core mod PostgreSQL:**
   * Konfigureres via EF Core Code-First med migrations.
   * Databasen skal seeds med relevante testdata ved opstart, så applikationen ikke er tom ved første kørsel.

### B. Blazor WebAssembly (Frontend)
1. **Komponentarkitektur:**
   * Brugerfladen skal opdeles logisk i genbrugelige Razor-komponenter (f.eks. listevisning, detaljekort, statistikkort, formular).
2. **API Klient & Separation of Concerns:**
   * Rå HTTP-kald må **ikke** foretages direkte i `.razor`-filernes kodeblokke.
   * I skal implementere en dedikeret service-klasse (f.eks. `ISubscriptionService` / `SubscriptionService` eller `IWorkoutService`), som injiceres i komponenterne via Dependency Injection.
3. **Formularer og Brugerinput:**
   * Al dataindtastning skal håndteres med Blazors `EditForm` samt `DataAnnotationsValidator` og `ValidationMessage` / `ValidationSummary`.
4. **Fejlhåndtering og Feedback:**
   * Brugeren skal have visuel feedback ved indlæsning (loading spinner/skelet), succeshandlinger og API-fejlbeskeder.

### C. .NET Aspire
1. **Orkestrering i AppHost:**
   * AppHost skal registrere PostgreSQL-databasen, Web API-projektet og Blazor-projektet.
   * Forbindelser skal defineres med `.WithReference(...)`:
     * API refererer databasen: `.WithReference(db)`
     * Blazor refererer API: `.WithReference(apiService)`
2. **Service Discovery:**
   * Blazor WASM skal kommunikere med Web API via det logiske navn angivet i Aspire (f.eks. `https://apiservice`) uden hårde localhost-porte i konfigurationsfiler.
3. **Telemetri & Overvågning:**
   * I skal demonstrere brug af **Aspire Dashboard** under afleveringen/forsvaret:
     * Visning af kørte containere og projekter.
     * Tracing af en forespørgsel fra Blazor gennem Web API og ned i PostgreSQL.
     * Visning af strukturerede logs.

---

## 6. Aflevering og Dokumentation

Besvarelsen kan afleveres som et samlet Git-repository indeholdende:
1. Den fulde kildekode til solutionen (`AppHost`, `ServiceDefaults`, `Web API`, `Blazor WASM` samt eventuelle shared biblioteker).
2. Database-migrations, så løsningen automatisk kan genskabe databasen på en ny maskine.
3. En kort `README.md` i roden af projektet med:
   * Angivelse af den valgte case (Case A eller B).
   * Forudsætninger (f.eks. Docker Desktop kørende).

