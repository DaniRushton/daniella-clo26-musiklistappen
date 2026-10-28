# Reflektion — Musiklistappen

**Namn:** Daniella
**Datum:** 2026-10-28

---

## Vad var svårast?

Jag hade problemet med att min databas inte kunde läsas in, så varje gång jag försökte läsa in koden för att se min progress så kunde jag inte köra programmet. Jag fattade inte att jag hade döpt databasfilen till musik.db istället för bara musik, så då vart det inte konstigt att programmet inte hittade filen. Istället så försökte den köra mot en tom databas i bin/ mappen. Efter att ha sparat databasen ett par hundra gånger och flyttat filen lite hit o dit så hittade jag eventuellt vad problemet var och jag kunde äntligen köra programmet efter att jag hade döpt om filen till bara "musik".

## Vad lärde du dig?

Att döpa filer korrekt. 
Annars lärde jag mig att koppla C# till en databas.

---

## Hur använde du ADO.NET i praktiken — vad kändes naturligt och vad var förvirrande?

Med SqliteConnection och connection.Open(); öppnas anslutningen mellan C# och databasen
Med connection.CreateCommand() så skapar jag ett kommando
Med command.CommandText så kan jag skriva en SQL kod
Med command.Parameters.AddWithValue() så skickar värden in på ett säkert sätt
Med ExecuteReader (för select) och ExecuteNonQuery (för insert/delete) körs kommandot i databasen 

DEt som var mer "naturligt" eller enklare var att det var i princip samma kod hela tiden så det enda jag egentligen behövde ändra på är självaste vad för SQL kod jag skulle skriva. Det som var mer förvirrande var skillnaden på ExecuteReader och ExecuteNonQuery, det hade jag inte riktigt koll på. 

---

## Varför ska man använda parameteriserade queries?

Vad kan hända om man klistrar ihop SQL med strängar i stället? (Tänk på Bobby Tables.)

För om man använder textsträngar så tolkas det användaren skriver som SQL kod, och om någon då skriver in exempelvis `'); DROP TABLE users; --` så kan hela tabeller eller en hel databas försvinna. Med parametrar så behandlar koden inputen som data istället för kod.

---

## Om du hade mer tid — vad hade du lagt till eller gjort annorlunda?

Kanske göra det lite mer interaktivt så att användaren kan välja va de vill se för information, istället för att ha en hel lista med låt titel, album id, låtlängd osv. Så att användaren kan välja att bara titta på en specifik låt, och också kunna se från vilket album den låten kommer från.

---

## (VG) Varför delade du upp koden som du gjorde?

`DbHandler.cs` tar hand om allt som har med databasen o göra, `Program.cs` tar hand om UI:n. Detta gör att jag kan ändra en SQL fråga i databasen utan att behöva meka med menyn, och vice versa. Hade allt legat tillsammans hade det nog blivit rörigare och svårare att hitta eventuella fel, och dessutom behövt skriva samma databaskod flera gånger.  