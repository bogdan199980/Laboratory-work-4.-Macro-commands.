using System;
using System.Collections.Generic;

namespace CommandPattern
{
    // Абстрактний клас команди
    abstract class Command
    {
        public abstract void Execute();
        public abstract void Undo();
    }

    // Клас конкретної команди
    class CalculatorCommand : Command
    {
        private char @operator;
        private int operand;
        private Calculator calculator;

        public CalculatorCommand(char @operator, int operand, Calculator calculator)
        {
            this.@operator = @operator;
            this.operand = operand;
            this.calculator = calculator;
        }

        public override void Execute()
        {
            calculator.Operation(@operator, operand);
        }

        public override void Undo()
        {
            calculator.Operation(UndoOperator(@operator), operand);
        }

        // Метод для відміни операції
        private char UndoOperator(char @operator)
        {
            switch (@operator)
            {
                case '+':
                    return '-';
                case '-':
                    return '+';
                case '*':
                    return '/';
                case '/':
                    return '*';
                default:
                    throw new ArgumentException("Unknown operator");
            }
        }
    }

    // Клас, який виконує операції калькулятора
    class Calculator
    {
        private int currentValue = 0;

        public void Operation(char @operator, int operand)
        {
            switch (@operator)
            {
                case '+':
                    currentValue += operand;
                    break;
                case '-':
                    currentValue -= operand;
                    break;
                case '*':
                    currentValue *= operand;
                    break;
                case '/':
                    currentValue /= operand;
                    break;
            }

            Console.WriteLine("Current value: " + currentValue);
        }
    }

    // Клас, який зберігає список команд та виконує їх
    class MacroCommand : Command
    {
        private List<Command> commands = new List<Command>();

        public void AddCommand(Command command)
        {
            commands.Add(command);
        }

        public override void Execute()
        {
            foreach (Command command in commands)
            {
                command.Execute();
            }
        }

        public override void Undo()
        {
            for (int i = commands.Count - 1; i >= 0; i--)
            {
                commands[i].Undo();
            }
        }
    }

    class Client
    {
        static void Main(string[] args)
        {
            Calculator calculator = new Calculator();

            // Створення команд
            CalculatorCommand addCommand = new CalculatorCommand('+', 5, calculator);
            CalculatorCommand multiplyCommand = new CalculatorCommand('*', 3, calculator);
            CalculatorCommand subtractCommand = new CalculatorCommand('-', 2, calculator);

            // Створення макрокоманди та додавання до неї
            // команд
            MacroCommand macroCommand = new MacroCommand();
            macroCommand.AddCommand(addCommand);
            macroCommand.AddCommand(multiplyCommand);
            macroCommand.AddCommand(subtractCommand);
            // Виконання макрокоманди
            macroCommand.Execute();

            // Відміна операцій
            macroCommand.Undo();

            Console.ReadKey();
        }
    }
}