int piezoPin;

void setup() {
  // put your setup code here, to run once:
  piezoPin = A0;
  Serial.begin(9600);
}

void loop() {
  // put your main code here, to run repeatedly:
  int piezoRead = analogRead(piezoPin);
  Serial.println(piezoRead);
  delay(20);
}