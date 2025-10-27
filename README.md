# Autószerelő Management Webalkalmazás

A projekt egy webes alkalmazás egy autószerelő műhely számára az ügyfelek és a munkák nyilvántartására.


## 🛠️ Tech Stack

* **Design:** Figma (UI/UX tervezés)
* **Backend:** ASP.NET Core Web API (.NET 9)
* **Frontend:** Blazor WebAssembly Standalone App (.NET 9)
* **Adatbázis:** PostgreSQL (Entity Framework Core 9 használatával)
* **Authentikáció:** ASP.NET Core Identity, JWT (JSON Web Tokens)

---

## ✅ Kurzus Követelmények

### 🌐 Webfejlesztés követelmények

* **2. szint:** Két típus (Customer, Job), DB perzisztencia, minden CRUD művelet megvalósítva, mindkét típus listázása
* **3. szint:** Mindkét típus CRUD a frontenden
* **4. szint:** JWT autentikáció
* **5. szint:** Extra funkciók:
    - .NET backend
    - Blazor frontend
    - Üzleti logika: munkaóra esztimáció, irányított munkafolyamat (To-Do -> In Progress -> Done)
    - Dark mode támogatás
    - Keresés, CSV export, Rendezés, Kártya / lista nézet

---

### 💻 Szoftverfejlesztés C# nyelven követelmények

* **Alapkövetelmények:** 
    - Git repo rendszeres commitokkal, .NET 9 használata
    - 1 solution: `CarMechanicManagementApp.sln`
    - 3 projekt:
        - ASP.NET Core Web API projekt (Server)
        - Blazor WebAssembly Standalone App projekt (Client)
        - Közös projekt (Shared) a modellek számára
    - Buildelhető és futtatható

* **Funkcionális:**
    - Jobs és Customers CRUD és lista nézet
    - Ügyfelekhez köthető munkák külön listázása
    - Munkaóra esztimáció számítása, megjelenítése

* **Technikai:**
    - UNIT tesztek a WebApi service-ekre *(Még hiányzik)*
    - Munkaóra esztimáció tesztelése *(Még hiányzik)*
    - Adatbázis kezelés EF használatával  
    - Model validáció (Front- és Backend)  
    - Specifikus validációk (Email, Rendszám, Év, Kategória, Súlyosság, Állapot)  
    - Munka állapota irányított váltása

* **Extra funkciók:**
    - JWT autentikációs beléptetés
    - Dark mode támogatás
    - Keresés, CSV export, Rendezés, Kártya / lista nézet

---

## 💾 Adatbázis Séma

Az alkalmazás két fő entitást kezel, melyek között egy-a-többhöz (1:N) kapcsolat van.

### Customer (Ügyfél)
| Oszlop | Típus | Leírás |
| :--- | :--- | :--- |
| **Id** (PK) | int | Elsődleges kulcs, auto-increment |
| Name | string | Ügyfél neve (kötelező) |
| Address | string | Lakcím (kötelező) |
| Email | string | Email cím (kötelező, email formátum) |

### Job (Munka)
| Oszlop | Típus | Leírás |
| :--- | :--- | :--- |
| **Id** (PK) | int | Elsődleges kulcs, auto-increment |
| **CustomerId** (FK) | int | Külső kulcs a `Customer` táblára (kötelező) |
| LicensePlateNumber | string | Rendszám (kötelező, `ABC-123` formátum) |
| VehicleYear | int | Gyártási év (1900-2100) |
| JobCategory | int (enum) | `Chassis`, `Engine`, `Suspension`, `Brakes` |
| VehicleIssueDescription | string | Hiba leírása (kötelező) |
| Severity | int | Súlyosság (1-10) |
| JobStage | int (enum) | `ToDo`, `InProgress`, `Done` |