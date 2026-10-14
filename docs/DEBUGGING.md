1. Jag undersökte fel 4, när användaren skriver in annat än siffra krashar programmet.

2. Återskapade problemet genom att starta programmet och skriva in något annat än en siffra där, jag skrev "six".

3. Jag fick en krash och felmeddelande: Exception Unhandled. System.FormatException: "The input string 'six' was not in a correct format.'

4. Jag ville få ett felmeddelande som skulle skrivits ut i konsollen i stil med "Please write a valid number and not in text how many participants"

5+6. Vi hade i tidigare kod "int.Parse" som försöker ändra string (som Console.WriteLine() blir) till ett talvärde. När då användaren skriver t.ex. "six" så kan inte int.Parse konvertera till tal. När den inte kan det så krashar programmet. Jag tror det är för att vi inte hanterar exceptionen som skapas då. 

7. Min ändring innebar en kanske okonventionell lösning men en lösning jag förstår: Jag skapade en while loop i Booking.Consol (kanske bör ha varit en egen metod egentligen) som körs om och om igen tills användaren skrivit en faktisk siffra. Jag kollar inmatat värde med TryParse istället för Parse. TryParse FÖRSÖKER konvertera till värde och returnerar en bool och ev. ett nytt värde beroende på om det lyckades eller ej. Om lyckades: true + värdet efter 'out'. Misslyckades (användaren skriver "six") så returneras false. På så sätt förhindrar vi krash. Sen kollar programmet om det blev lyckat (true) och tar oss då ur loopen, annars skrivs felmeddelande ut och loopen börjar om.

8. Jag startade programmet och skapade en ny bokning. Väl vid participants skrev jag "six" igen och fick då felmeddelande direkt utan att programmet krashade. Jag testade att skriva igen massa nonsens och loopen gick om. Tillslut skrev jag siffra "2" och programmet gick vidare med bokningen.

9. Det finns ingen som **direkt** testar om vi har en siffra och inte text. Men vi har iallafall två tester som är associerade med det. Dock när jag testade att sätta en vanlig parse som inte granterar siffra så gick de ändå igenom. Vi skulle nog behöva ett nytt test för denna. 
- "Create_WhenParticipantCountExceedsCapacity_ReturnFailure" (om ingen siffra kan den inte kolla om den är större)
- "Create_WhenBookingIsValid_AddsAndSavesBooking" (om ej siffra kan den ej skapa bokningen)
