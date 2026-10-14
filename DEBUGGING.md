1. Jag undersökte fel 4, när användaren skriver in annat än siffra krashar programmet.

2. Återskapade problemet genom att starta programmet och skriva in något annat än en siffra där -> fick krash och felmeddelande: Exception Unhandled. System.FormatException: "The input string 'six' was not in a correct format.'

3. Jag ville få att ett felmeddelande skrivs ut i stil med "Please write a valid number and not in text how many participants"

4+5. Vi hade i tidigare kod "int.Parse" som försöker ändra text (som Console.WriteLine() är) till ett talvärde. När då användaren skriver t.ex. "six" så kan inte int.Parse konvertera till tal. När den inte kan det så krashar programmet för att vi inte fångar upp exceptionen.  

6. Min ändring innebar en kanske okonventionell lösning men en lösning jag förstår: Jag skapade en while loop i Booking.Consol (kanske bör ha varit en egen metod egentligen) som körs om och om igen tills användaren skrivit en faktisk siffra. Jag kollar inmatad värde med TryParse istället för Parse. TryParse FÖRSÖKER konvertera till värde och returnerar en bool och ev. ett nytt värde beroende på om det lyckades eller ej. Om lyckades: true + värdet efter out. Misslyckades (användaren skriver "six") så returneras false. På så sätt förhindrar vi krash. Jag kollar om det blev lyckat (true) och tar oss då ur loopen, annars skrivs felmeddelande ut och loopen börjar om. 
