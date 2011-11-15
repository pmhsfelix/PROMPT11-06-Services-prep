# Session's script #

<!--- ------------------------------ Session 1 ------------------------------ -->
## Session 1, 16/11/2011 ##

### Goals ###

* Introduction to the WCF programming model 
* Introduction to the SOAP model
	
### Outline ###

* Brief WCF introduction: goals and history
* The first service - WCF programming model
	* DictionaryService (alternatives: ?)
	* 5-step impl: contracts, impl, hosting, behaviors
	* Service usage: options {custom client, WCF test client, other generic client (SoapUI)}
	
* Practice: implement the *same* service from theory and use it via the SOAP client
	
* What is happening behind the scenes - The SOAP model 
	* SOAP envelopes
	* WSDL description
		* Relation to the WCF programming model
		
* WCF client programming model
* The svcutil tool and the "add service reference"

* Practice: consume a service
	* ~~services.sapo.pt (e.g. EPG-Meo)~~
	* bing maps SOAP API
	* http://www.holidaywebservice.com/
	* http://services.aonaware.com/DictService/DictService.asmx
	
* Practice: implement a service
	* TV programmes showing on a giving moment (sorted by preference?)	

<!--- ------------------------------ Session 2 ------------------------------ -->
## Session 2, 17/11/2011 ##

### Goals ###

* Continue previous session
* WCF runtime architecture and description model

### Outline ###

* Hosting on IIS (movable to previous)
* XML configuration (movable to previous)
	* Using the service configuration editor

* [18:30-20:00] Continue practice from previous day

* WCF runtime architecture
	* Overview - two layers
	* Channel stack layer
		* The Message type
		* Message processing
		* Protocol channels
		* Transport channels and encoders
		* Description via binding elements
		* Demo: using XMPP|SMTP to transport messages
		* !Quotas and binding parameters!
	* Service model layer
		* from messages to calls (instances, methods, parameters)
		* runtime organization
		
* Service description
	* ...	

* Practice
	* Description analyzer (MVC "endpoints")
	* Custom binding (using existent binding elements) - ???
	* Behavior (add and consume custom headers)

<!--- ------------------------------ Session 3 ------------------------------ -->
## Session 3, 23/11/2011 ##

### Goals ###

* Service design (follow Lowy's book)
	* Service contracts
	* Data contracts
	* Operation contracts
	* Faults

### Outline ###

* [Read Lowy's book]

<!--- ------------------------------ Session 4 ------------------------------ -->
## Session 4, 24/11/2011 ##

### Goals ###
* Using WCF to write intermediaries
* AppFabric Service Bus

### Outline ###

<!--- ------------------------------ Session 5 ------------------------------ -->
## Session 5, 30/11/2011 ##

### Goals ###
* Introduction to "REST" web services - using HTTP as the application protocol
* The WCF web Api

### Outline ###
* The web architecture
	* Resources
	* URIs
	* Representations
	* HTTP
* HTTP revisions
	* Request and response messages
	* Message entity/payload, media-types
	* HTTP methods
* Using HTTP to build web services
	* ...
* The WCF web api

<!--- ------------------------------ Session 6 ------------------------------ -->
## Session 6, 01/12/2011 ##

### Goals ###

### Outline ###

<!--- ------------------------------ Session 7 ------------------------------ -->
## Session 7, 07/12/2011 ##

### Goals ###

### Outline ###

<!--- ------------------------------ Session 8 ------------------------------ -->
## Session 8, 08/12/2011 ##

### Goals ###

### Outline ###
