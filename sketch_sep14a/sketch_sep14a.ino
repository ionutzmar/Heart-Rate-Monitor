
const int inputPin = A0;
byte inputByte_0;

byte inputByte_1;

byte inputByte_2;

byte inputByte_3;

byte inputByte_4;

void setup() {
  
  Serial.begin(9600);
}

void loop() {
  
  Serial.println(analogRead(inputPin));

  if (Serial.available() == 5) {
            //Read buffer
            inputByte_0 = Serial.read();
            delay(100);
            inputByte_1 = Serial.read();
            delay(100);
            inputByte_2 = Serial.read();
            delay(100);
            inputByte_3 = Serial.read();
            delay(100);
            inputByte_4 = Serial.read();
        }
        if (inputByte_0 == 16) {
            //Detect Command type
            if (inputByte_1 == 128) {
                Serial.write("Hello budie!");

            }
            //Clear Message bytes
            inputByte_0 = 0;
            inputByte_1 = 0;
            inputByte_2 = 0;
            inputByte_3 = 0;
            inputByte_4 = 0;
        }
}
