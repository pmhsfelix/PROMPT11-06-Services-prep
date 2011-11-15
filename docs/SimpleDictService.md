# Simple Dictionary Service 

* Introduction to WCF's programming model

* Create a console app project

* Change target framework to .NET 4 (not client profile)

* Add references to "System.ServiceModel" and "System.Runtime.Serialization"

* Install SOAPUI (32-bit)

* Request message

<soapenv:Envelope 
		xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:ser="http://prompt.cc.isel.ipl.pt/services">
   <soapenv:Header/>
   <soapenv:Body>
      <ser:WordLookupOp>
         <!--Optional:-->
         <ser:Request>
            <!--Optional:-->
            <ser:Word>?</ser:Word>
         </ser:Request>
      </ser:WordLookupOp>
   </soapenv:Body>
</soapenv:Envelope>

* The response

<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
   <s:Body>
      <WordLookupOpResponse xmlns="http://prompt.cc.isel.ipl.pt/services">
         <WordLookupOpResult xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
            <Definition>Aquele que ensina uma arte, uma actividade, uma ciência, uma língua, etc.</Definition>
            <Exists>true</Exists>
         </WordLookupOpResult>
      </WordLookupOpResponse>
   </s:Body>
</s:Envelope>

* The alternative response

<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
   <s:Body>
      <WordLookupOpResponse xmlns="http://prompt.cc.isel.ipl.pt/services">
         <WordLookupOpResult xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
            <Exists>false</Exists>
         </WordLookupOpResult>
      </WordLookupOpResponse>
   </s:Body>
</s:Envelope>
