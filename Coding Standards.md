# Coding Standards
## Team 5
## Rick Bowman, Quinn Nimmer, Mike Schommer, Seth Frevert

### **C#**
- Naming Conventions
    - PascalCase
        - Classes
        - Methods
        - Enumerations
        - Public fields and properties
        - Namespaces
        - File names
    - camelCase
        - Local variables
        - Parameters
    - camelCase with _ prefix
        - Private, protected, internal, and protected internal fields and properties
- Interface names being with I
    - IDatabase, IAnimal
- Do not use prefixes in file names
    - Correct: User, UserTest
    - Incorrect: cUser (class), utUser (unit test)
- When using a Switch statement, remove the default case if it will never be reached
- Methods longer than 20 lines should be refactored if possible
- Methods can be a maximum of 30 lines long
- User American English spellings
    - Correct: color
    - Incorrect: colour
- Braces belong on their own line
    - Correct:
        -class MyClass
        -{
            -void DoSomething()
            -{
                -if (condition)
                -{
                -}
                -else
                -{
                -}
            -}
        -}
    - Incorrect:
        class MyClass{
            void DoSomething(){
                if (condition){
                } else {
                }
            }
        }
- Use braces even if they are optional
    - Correct:
        if (condition)
        {
            return true;
        }
    - Incorrect:
        if (condition)
            return true;
- Write only one statement per line
    - Correct:
        int a = 1;
        int b = 2;
    - Incorrect:
        int a = 1; int b = 2;
-
