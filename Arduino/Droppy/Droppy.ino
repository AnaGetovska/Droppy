#include <ESP8266HTTPClient.h>

#include <ESP8266WiFi.h>
#include <WiFiClient.h>

// Replace with your network credentials
const char* ssid = "Munchkins";
const char* password = "SECRET";
const int AirValue = 1024;
const int WaterValue = 620;

float soilMoisture = 0;

String guid = "20cda1bd-e4ea-4ee3-9a0a-cccef03cdd7f";


void setup(void) {
  pinMode(A0, INPUT);
  Serial.begin(115200);

  // Init WiFi
  WiFi.hostname("Droppy1");
  WiFi.begin(ssid, password); //begin WiFi access point
    while (WiFi.status() != WL_CONNECTED)
  {
    delay(200);
    Serial.print(".");
  }
  Serial.println();

  Serial.print("Connected, IP address: ");
  Serial.println(WiFi.localIP());
  Serial.println("");
  delay(100);
  Serial.print("Sending: ");
  Serial.println(soilMoisture / 100);
  
  readMoisture();
  HTTPClient http;
  http.begin("http://droppy.internal.st/api/update");
  http.addHeader("Content-Type", "application/x-www-form-urlencoded");
  String httpRequestData = "device=Test2&reading=" + String(soilMoisture / 100);
  int httpResponseCode = http.POST(httpRequestData);
  Serial.print("Response: ");
  Serial.println(httpResponseCode);
  
  ESP.deepSleep(2e7); // 20 sec
}

void readMoisture() {
  int moisture = analogRead(A0);
  soilMoisture = map(moisture, AirValue, WaterValue, 0, 100);
  Serial.println(soilMoisture);
}

void loop(void) {
  //soilMoisture = map(analogRead(A0), AirValue, WaterValue, 0, 100);
  //server.handleClient();
}
