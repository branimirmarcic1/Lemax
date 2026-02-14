# Lemax Hotel Management & Search System

![Build Status](https://github.com/branimirmarcic1/Lemax/actions/workflows/main.yml/badge.svg)

Ovaj projekt predstavlja robusno **ASP.NET Core** rješenje za upravljanje hotelima i naprednu pretragu temeljenu na lokaciji korisnika. Glavni fokus sustava je balansiranje cijene i udaljenosti kako bi korisnik dobio optimalne rezultate.

---

## 🏗️ Arhitektura sustava

Projekt je implementiran koristeći principe **Clean Architecture** i podijeljen je na sljedeće slojeve:

* **Lemax.Domain**: Sadrži entitete i osnovne definicije.
* **Lemax.Application**: Poslovna logika, DTO-ovi, sučelja i validacija (FluentValidation).
* **Lemax.Infrastructure**: Implementacija baze (EF Core), servisa za udaljenost (Haversine formula), mapiranja (Mapster) i middleware-a.
* **Lemax.Api**: Entry point aplikacije s kontrolerima i Swagger dokumentacijom.
* **Lemax.UnitTests**: Skupina testova za validaciju logike i pokrivenosti koda.

---

## 🚀 Ključne funkcionalnosti

* **Puni Hotel CRUD**: Kreiranje, pregled, ažuriranje i brisanje hotela.
* **Napredni Search**: Pretraga hotela prema koordinatama korisnika (Latitude/Longitude).
* **Algoritam rangiranja**: Sustav koristi formulu za izračunavanje "score-a" svakog hotela:
  $$Score = \text{Price} + \text{Distance (km)}$$
  *Hoteli s manjim zbrojem (bliži i jeftiniji) pojavljuju se prvi na listi.*
* **Paginacija**: Svi rezultati pretrage su paginirani s metapodacima (`TotalCount`, `TotalPages`).
* **Globalno upravljanje greškama**: Custom Middleware za konzistentne JSON odgovore.

---

## 🛠️ Tehnologije

* **.NET 8.0**
* **Entity Framework Core** (In-Memory provider)
* **FluentValidation** & **Mapster**
* **Serilog** (Strukturirano logiranje)
* **xUnit**, **FluentAssertions** & **Coverlet**

---

## ⚙️ Instalacija i pokretanje

1. **Klonirajte repozitorij:**
   ```bash
   git clone [https://github.com/branimirmarcic1/Lemax.git](https://github.com/branimirmarcic1/Lemax.git)
