#include <LiquidCrystal.h>
#define MAX_LINE_LENGTH 34
char buffer[MAX_LINE_LENGTH];
LiquidCrystal lcd(12, 11, 5, 4, 3, 2);
String leitura[3];
int counter = 0;
void setup() {
  Serial.begin(9600);
  pinMode(LED_BUILTIN, OUTPUT);
  digitalWrite(LED_BUILTIN, LOW);
  lcd.begin(16, 2);
  welcome();
  
}
void welcome(){
  counter = 0;
  lcd.clear();
  lcd.setCursor(0,0);
  lcd.print("Hello!");
  lcd.setCursor(0,1);
  lcd.print("Waiting for data.");
  delay(500);
}
void loop() {
   if (readLine() > 0) 
  {
    counter = 0;
    lcd.clear();
    int pos = 0;
    char *token = strtok(buffer, "_"); //Start tokenization
    while (token) //While a token is available
    {
      leitura[pos] = token;
      token = strtok(NULL, "_"); //Fetch next token
      pos++;
    }
    
  lcd.setCursor(0,0);
  lcd.print(leitura[0]);
  lcd.setCursor(0,1);
  lcd.print(leitura[1]);
  }else{
    if(counter == 25){
    welcome();
    }else{
      counter++;
    }
  }
  delay(300);
}
int readLine()
{
  int cc = 0; //Number of chars read
  bool done = false;
  while (Serial.available() && (!done) && (cc < MAX_LINE_LENGTH - 1))
  {
    char cur = Serial.read(); //Read a char
    if (cur == '\n') done = true; //If the received char is \n then we are done
    else buffer[cc++] = cur; //Append to buffer and increment the index
  }
  buffer[cc] = 0; 
  return cc; 
}
