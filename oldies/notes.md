# CSharp Shots, Tagus, 2009 #

## Slides ##
	* Introduction to WCF: goals, history,...
	* Programming model
		* Example: dictionary
		* 5 steps: service contract, data contract, service impl, service hosting, behaviors
		* main concepts: contracts, service impl., hosts, endpoint = contract+binding+address
		* Client-side programming model
		* Using the google search service (deprecated!)
		* Metadata, WSDL and svcutil
		* Configuration files
	* Execution architecture
		* Service Model Layer + Channel Model Layer
		* The message type
		* Messages, protocol channels, transport channels and its relation to SOAP envelopes
		* Description model
		* Bindings
		* Dispatcher
		* Behaviors
		* Bindings vs. behaviors
	* Web Programming Model
	* Service bus
## Code ##
	* DictionaryService
	* DictionaryClient
	* GoogleClient -spelling (deprecated)
	* Messaging
		* One-way messaging using various transports: http, service bus, XMPP, SMTP
	* SmtpTransport
	* XmppTransport
	* WebProgrammingModel
	* GetScreen - Service bus demo
	* custom behavior - RequireNonNull attribute

# TechDays, 2008 #

## Slides ##
	* What is WCF
	* Execution architecture
		* The 2 layers
		* The Message type
		* Channels and messages		
	* Service Description		
	* Bindings and Channel Stack
		* Custom binding using binding elements
	* Dispatching
	* Behaviors
		* Bindings vs.behaviors
		
## Code ##
	* SMTP and XMPP transports
	* Messaging and multiple transports
	* RequireNonNull custom behavior


