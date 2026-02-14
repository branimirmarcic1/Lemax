Evo kompletnog i profesionalnog **README.md** dokumenta koji možeš odmah ubaciti u root svog projekta. Napisan je tako da ostavi odličan dojam na onoga tko ga bude čitao, jer jasno objašnjava arhitekturu, logiku i način testiranja.

---

# Lemax Hotel Management & Search System

Ovaj projekt predstavlja robusno **ASP.NET Core** rješenje za upravljanje hotelima i naprednu pretragu temeljenu na lokaciji korisnika. Glavni fokus sustava je balansiranje cijene i udaljenosti kako bi korisnik dobio optimalne rezultate.

---

## 🏗️ Arhitektura sustava

Projekt je implementiran koristeći principe **Clean Architecture** i podijeljen je na sljedeće slojeve:

* **Lemax.Domain**: Sadrži entitete i osnovne definicije.
* **Lemax.Application**: Poslovna logika, DTO-ovi, sučelja (Interfaces) i validacija (FluentValidation).
* **Lemax.Infrastructure**: Implementacija baze (EF Core), servisa za udaljenost (Haversine formula), mapiranja (Mapster) i middleware-a.
* **Lemax.Api**: Entry point aplikacije s kontrolerima i Swagger dokumentacijom.
* **Lemax.UnitTests**: Skupina testova za validaciju logike i pokrivenosti koda.

---

## 🚀 Ključne funkcionalnosti

* **Puni Hotel CRUD**: Kreiranje, pregled, ažuriranje i brisanje hotela.
* **Napredni Search**: Pretraga hotela prema koordinatama korisnika (Latitude/Longitude).
* **Algoritam rangiranja**: Sustav koristi formulu za izračunavanje "score-a" svakog hotela:



*Hoteli s manjim zbrojem (bliži i jeftiniji) pojavljuju se prvi na listi.*
* **Paginacija**: Svi rezultati pretrage i listanja su paginirani s metapodacima (`TotalCount`, `TotalPages`, `HasNextPage`).
* **Globalno upravljanje greškama**: Custom Middleware za hvatanje iznimaka i konzistentne JSON odgovore.
* **Validacija podataka**: Stroga pravila za geografske koordinate i cijene.

---

## 🛠️ Tehnologije

* **.NET 8.0**
* **Entity Framework Core** (In-Memory provider za testiranje)
* **FluentValidation** (Validacija requestova)
* **Mapster** (High-performance objekt-na-objekt mapiranje)
* **Serilog** (Strukturirano logiranje)
* **xUnit & FluentAssertions** (Unit testiranje)
* **Coverlet & ReportGenerator** (Code coverage izvještaji)

---

## ⚙️ Instalacija i pokretanje

1. **Klonirajte repozitorij:**
```bash
git clone https://github.com/branimirmarcic1/Lemax.git

```


2. **Restore paketa i Build:**
```bash
dotnet build

```


3. **Pokretanje aplikacije:**
```bash
dotnet run --project src/Lemax.Api

```


Aplikacija će po defaultu biti dostupna na `https://localhost:7081/swagger`.

---

## 🧪 Testiranje i Code Coverage

Sustav ima visoku pokrivenost unit testovima za ključnu poslovnu logiku i validatore.

**Pokretanje testova:**

```bash
dotnet test --collect:"XPlat Code Coverage"

```

**Generiranje HTML izvještaja o pokrivenosti:**

```bash
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html

```

*Izvještaj će biti dostupan u mapi `coveragereport/index.html`.*

---

## 📮 Postman simulacija (Primjer pretrage)

Za testiranje balansa cijene i udaljenosti, koristite sljedeće parametre:

* **URL:** `GET /api/hotels/search`
* **Params:**
* `latitude`: `45.8060` (Glavni kolodvor Zagreb)
* `longitude`: `15.9780`
* `page`: `1`
* `pageSize`: `5`



**Očekivani poredak:**

1. **Chillout Hostel** (30€, ~1km udaljenosti) - *Pobjednik zbog niske cijene.*
2. **Swanky Mint Hostel** (35€, ~1km udaljenosti).
3. **Best Western Astoria** (85€, ~0.2km udaljenosti) - *Iako je najbliži, cijena ga spušta ispod hostela.*
4. **Esplanade Hotel** (180€, ~0.1km udaljenosti) - *Zadnji zbog visoke cijene unatoč idealnoj lokaciji.*

---

Ovaj projekt služi kao dokaz primjene modernih praksi u razvoju .NET web servisa. Sve komponente su dizajnirane tako da se lako mogu proširiti ili zamijeniti (npr. prelazak na SQL Server umjesto In-Memory baze).
