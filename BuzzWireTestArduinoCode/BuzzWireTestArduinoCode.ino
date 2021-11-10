// constants won't change. They're used here to set pin numbers:
     // the number of the pushbutton pin
const int left_rest_pin = 3; 
const int right_rest_pin = 2; 
const int mistake_pin = 5; 

// variables will change:

int left_rest_state = 0; 
int right_rest_state = 0; 
int mistake_state = 0; 

void setup() 
{
  Serial.begin(9600); 

  pinMode(left_rest_pin, INPUT);
  pinMode(right_rest_pin, INPUT);
  pinMode(mistake_pin, INPUT);
}

void loop() 
{
  // read the state of the pushbutton value:
  left_rest_state = digitalRead(left_rest_pin);
  right_rest_state = digitalRead(right_rest_pin);
  mistake_state = digitalRead(mistake_pin);
  
  // check if the pushbutton is pressed. If it is, the buttonState is HIGH:
  if (left_rest_state == HIGH) 
  {
    Serial.println("+");
  } 

  if (right_rest_state == HIGH) 
  {
    Serial.println("*");
  } 

  if (mistake_state == HIGH) 
  {
    Serial.println("1");
  }   
}
