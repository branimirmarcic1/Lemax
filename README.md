# Lemax Hotel Management & Search System

Ovaj projekt predstavlja robusno **ASP.NET Core** rješenje za upravljanje hotelima i naprednu pretragu temeljenu na lokaciji korisnika. Glavni fokus sustava je balansiranje cijene i udaljenosti kako bi korisnik dobio optimalne rezultate prema zadanoj formuli ranga.

---

## 🏗️ Arhitektura sustava

Projekt prati **Clean Architecture** principe, s jasno odvojenim slojevima radi lakšeg održavanja i skaliranja:

* **Core (Lemax.Domain & Lemax.Application)**: Srce sustava koje sadrži poslovnu logiku, entitete i sučelja.
* **Infrastructure (Lemax.Infrastructure)**: Implementacija tehničkih detalja: EF Core, Identity servisi, Haversine formula i globalni Error Handling.
* **Migrators (Lemax.SQL)**: Zaseban projekt zadužen za upravljanje SQL Server migracijama, čime se osigurava čistoća infrastrukture.
* **Presentation (Lemax.API)**: REST endpointi grupirani pod `/api` prefiksom radi konzistentnosti.

---

## 🔐 Sigurnost (Authentication & Authorization)

Sustav implementira cjeloviti sigurnosni okvir koristeći **ASP.NET Core Identity**:

* **Autentifikacija**: Korisnici se mogu registrirati (`/api/register`) i prijaviti (`/api/login`) kako bi dobili **JWT Bearer Token**.
* **Autorizacija**: Pristup kritičnim operacijama poput brisanja hotela zaštićen je **Role-based** pristupom (rola `Admin`).
* **Centralizirane konstante**: Svi admin podaci i ključne postavke definirane su u `Lemax.Shared` projektu radi lakše promjene na jednom mjestu.

---

## 🚀 Ključne funkcionalnosti i Algoritam

Glavna odlika sustava je **Search** funkcionalnost koja rangira hotele prema sljedećoj logici:

Algoritam izračunava zračnu udaljenost između korisnika i hotela pomoću Haversine formule, zbraja je s cijenom noćenja te sortira rezultate od najmanjeg prema najvećem zbroju.

---

## 🛠️ Tehnologije

* **.NET 8.0**
* **Entity Framework Core** (SQL Server & In-Memory podrška)
* **ASP.NET Core Identity** (JWT Bearer Tokeni)
* **FluentValidation & Mapster**
* **Serilog** (Strukturirano logiranje)
* **xUnit & FluentAssertions** (Unit testiranje)

---

## 🐳 Docker i Monitoring

Aplikacija je u potpunosti kontejnerizirana. Infrastrukturni monitoring (Health Check) dostupan je na: `http://localhost:8080/api/health`. Ovaj endpoint je javan kako bi ga vanjski sustavi za monitoring mogli nesmetano pozivati.

---

## 📮 Postman Kolekcija i Okruženja

Za testiranje je pripremljena napredna Postman kolekcija koja koristi **Environments** za automatsko prebacivanje između okruženja:

### 🌍 Dostupna okruženja:

1. **Localhost**: Cilja izravni razvojni endpoint na `https://localhost:7021/api`.
2. **Docker**: Cilja kontejneriziranu aplikaciju na `http://localhost:8080/api`.

### 🤖 Automatizacija:

* Kolekcija sadrži **Post-response skripte** koje automatski hvataju `accessToken` nakon prijave i spremaju ga u varijablu `{{token}}`.
* Svi zaključani zahtjevi automatski nasljeđuju autentifikaciju s nivoa kolekcije, što omogućuje besprijekorno testiranje bez ručnog kopiranja tokena.

---

## 🤖 AI Utilization

Sukladno zahtjevima, u razvoju rješenja korišteni su AI asistenti (ChatGPT/Gemini) za sljedeće zadatke:

* **Arhitektura migracija**: Strategija odvajanja SQL migracija u zaseban `Lemax.SQL` projekt unutar `Migrators` mape.
* **Identity & Swagger**: Rješavanje kolizija ruta pri mapiranju Identity endpointova te konfiguracija Swaggera za ispravan prikaz Bearer Token polja.
* **Route Grouping**: Implementacija `MapGroup("/api")` za postizanje konzistentne strukture URL-ova i logičko grupiranje dokumentacije.
* **Environment Logic**: Pomoć u definiranju logike za micanje "lokota" s javnih endpointova poput `/health` uz istovremeno zaključavanje poslovne logike.

---

## ⚙️ Lokalni razvoj i Baza

Aplikacija podržava rad s pravom bazom putem EF Core migracija:

1. **Dodavanje migracije**: `dotnet ef migrations add <Ime> -p src/Lemax.SQL -s src/Lemax.API`.
2. **Update baze**: `dotnet ef database update -p src/Lemax.SQL -s src/Lemax.API`.
