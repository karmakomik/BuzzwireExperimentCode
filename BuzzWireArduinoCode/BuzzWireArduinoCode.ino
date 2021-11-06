// constants won't change. They're used here to set pin numbers:
     // the number of the pushbutton pin
const int level_4_right_pin = 3; 

const int level_4_touch_pin = 5; 

const int redLedPin =  10;      // the number of the LED pin
const int greenLedPin =  12;      // the number of the LED pin

// variables will change:

int level_4_right_button_state = 0; 
int level_4_touch_state = 0; 

void setup() 
{
  Serial.begin(9600);
  
  // initialize the LED pin as an output:
  pinMode(redLedPin, OUTPUT);
  pinMode(greenLedPin, OUTPUT);
    
  // initialize the pushbutton pin as an input:

  pinMode(level_4_right_pin, INPUT);
  pinMode(level_4_touch_pin, INPUT);
}

void loop() 
{
  // read the state of the pushbutton value:
  level_4_right_button_state = digitalRead(level_4_right_pin);
  level_4_touch_state = digitalRead(level_4_touch_pin);
  
  // check if the pushbutton is pressed. If it is, the buttonState is HIGH:
  if (level_4_right_button_state == HIGH) 
  {
    // turn LED on:
    digitalWrite(greenLedPin, HIGH);
    Serial.println("+");
  } 
  else 
  {
    // turn LED off:
    digitalWrite(greenLedPin, LOW);
    Serial.println("-");
  }

  if (level_4_touch_state == HIGH) 
  {
    // turn LED on:
    digitalWrite(redLedPin, HIGH);
    Serial.println("1");
  } 
  else 
  {
    // turn LED off:
    digitalWrite(redLedPin, LOW);
    Serial.println("0");
  }
  
}
