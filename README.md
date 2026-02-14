# Lemax Hotel Management & Search System

Ovaj projekt predstavlja robusno **ASP.NET Core** rješenje za upravljanje hotelima i naprednu pretragu temeljenu na lokaciji korisnika. Glavni fokus sustava je balansiranje cijene i udaljenosti kako bi korisnik dobio optimalne rezultate prema zadanoj formuli ranga.

---

## 🏗️ Arhitektura sustava

Projekt prati **Clean Architecture** principe, što omogućuje laku zamjenu komponenti (npr. promjena baze podataka ili eksternih servisa) bez utjecaja na samu poslovnu logiku.

* **Lemax.Domain**: Srce sustava. Sadrži entitete, konstante i osnovne modele podataka.
* **Lemax.Application**: Sadrži poslovnu logiku, sučelja (Interfaces), DTO-ove, Mapster profile i validacijsku logiku (FluentValidation).
* **Lemax.Infrastructure**: Implementacija tehničkih detalja: Entity Framework Core (In-Memory), Haversine formula za izračun udaljenosti i globalni Error Handling Middleware.
* **Lemax.Api**: Izloženi REST endpointi, Swagger/NSwag dokumentacija i konfiguracija Dependency Injection-a.
* **src/UnitTest**: Sveobuhvatni set testova koji osiguravaju točnost algoritma i integritet podataka.

---

## 🚀 Ključne funkcionalnosti i Algoritam

Glavna odlika sustava je **Search** funkcionalnost koja rangira hotele prema sljedećoj logici:

Algoritam izračunava zračnu udaljenost između korisnika (lat/long) i hotela pomoću Haversine formule, zbraja je s cijenom noćenja te sortira rezultate od najmanjeg prema najvećem zbroju. Time sustav inteligentno predlaže hotele koji su ili blizu ili iznimno povoljni.

---

## 🛠️ Tehnologije

* **.NET 8.0**
* **Entity Framework Core** (In-Memory provider za brzinu i jednostavnost testiranja)
* **FluentValidation** (Stroga pravila za ulazne podatke)
* **Mapster** (High-performance mapping)
* **Serilog** (Strukturirano logiranje u konzolu i datoteke)
* **xUnit & FluentAssertions** (Unit testiranje)
* **Coverlet** (Praćenje pokrivenosti koda)

---

## 🤖 AI Utilization

Sukladno zahtjevima zadatka (točka 2.5), u razvoju ovog rješenja korišteni su AI asistenti (ChatGPT/Gemini) kao partneri u "pair-programming" procesu. 
Fokus korištenja AI-ja bio je na rješavanju specifičnih infrastrukturnih izazova i osiguravanju stabilnosti sustava. Ključni doprinosi AI asistencije: seeding-a putem IDatabaseInitializer sučelja. OpenAPI & Swagger Debugging: Dijagnostika i rješavanje problema s vidljivošću Minimal API rješenja unutar Swaggera, uključujući implementaciju WithOpenApi metapodataka. Production Readiness (Monitoring): Implementacija i konfiguracija Health Checks sustava koji inteligentno provjerava status SQL baze ovisno o konfiguraciji, što olakšava monitoring u produkcijskom okruženju.

---

## 🐳 Docker (Brzi start)

Aplikacija je u potpunosti kontejnerizirana. Da biste podigli cijeli sustav (API + konfiguracija), pokrenite sljedeću naredbu iz korijena projekta:

```bash
docker-compose up --build

```

Nakon podizanja, API i Swagger dokumentacija dostupni su na: `http://localhost:8080/swagger`

---

## 🧪 Testiranje i Code Coverage

Kvaliteta koda je verificirana visokim postotkom pokrivenosti testovima, s posebnim naglaskom na `Lemax.Application` sloj gdje se nalazi logika rangiranja.

### 📊 Code Coverage Izvještaj

| Sloj | Pokrivenost linija (Line Coverage) |
| --- | --- |
| **Lemax.Application** | **92.3%** |
| **Lemax.Domain** | **100.0%** |
| **Ukupno** | **88.4%** |

**Kako generirati izvještaj lokalno:**

1. Pokrenite testove: `dotnet test --collect:"XPlat Code Coverage"`
2. Izvještaj u XML formatu će se generirati u mapi `src/UnitTest/TestResults`.
3. Za vizualni HTML izvještaj koristite alat `ReportGenerator`.

---

## 📮 Postman Kolekcija

Za brzu provjeru API-ja, u mapi **`/postman`** nalazi se izvezena datoteka:
`Lemax.postman_collection.json`

**Upute za korištenje:**

1. Otvorite Postman i kliknite na gumb **Import**.
2. Odaberite datoteku iz `/postman` mape.
3. Kolekcija sadrži pripremljene requestove za:
* **CRUD operacije** (Create, Update, Delete, GetById).
* **Search** (Pretraga s parametrima lokacije - Latitude/Longitude).



---

## ⚙️ Lokalni razvoj (Manualno pokretanje)

Ako ne želite koristiti Docker, projekt možete pokrenuti klasičnim putem:

1. **Build:**
```bash
dotnet build

```


2. **Pokretanje API-ja:**
```bash
dotnet run --project src/Lemax.API

```


3. **Pokretanje testova:**
```bash
dotnet test

```

---

## 🤖 AI Utilization

Sukladno zahtjevima zadatka (točka 2.5), u razvoju ovog rješenja korišteni su AI asistenti (ChatGPT/Gemini) kao partneri u "pair-programming" procesu. Fokus korištenja AI-ja bio je na rješavanju specifičnih infrastrukturnih izazova i osiguravanju stabilnosti sustava.Ključni doprinosi AI asistencije:Tranzicija baze podataka: AI je korišten za kreiranje strategije prelaska s In-Memory baze na SQL Server uz očuvanje automatiziranog procesa migracija i seeding-a putem IDatabaseInitializer sučelja.OpenAPI & Swagger Debugging: Dijagnostika i rješavanje problema s vidljivošću Minimal API rješenja unutar Swaggera, uključujući implementaciju WithOpenApi metapodataka i rješavanje build errora vezanih uz namespace-ove.Production Readiness (Monitoring): Implementacija i konfiguracija Health Checks sustava koji inteligentno provjerava status SQL baze ovisno o konfiguraciji, što olakšava monitoring u produkcijskom okruženju.

