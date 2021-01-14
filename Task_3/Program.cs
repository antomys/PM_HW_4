using System;

namespace Task_3
{
    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine("Notes, Volokhovych");
            Menu();
        }

        private static void Menu()
        {
            Console.WriteLine("1. Search Note");
            Console.WriteLine("2. Watch Notes");
            Console.WriteLine("3. Create Note");
            Console.WriteLine("4. Delete Note");
            Console.WriteLine("5. Exit");
            int input;
            string stringInput;
            do
            {
                Console.Write("Choose: ");
                stringInput = Console.ReadLine();
                Int32.TryParse(stringInput, out input);
            } while (input <= 0 || input > 5 || string.IsNullOrEmpty(stringInput));
            switch (input)
            {
                case 1:
                    SearchNote();
                    break;
                case 2:
                    WatchNote();
                    break;
                case 3:
                    AddNote();
                    break;
                case 4:
                    DeleteNote();
                    break;
                case 5:
                    Environment.Exit(1);
                    break;
            }
        }

        private static void DeleteNote()
        {
            int input;
            string stringInput;
            do
            {
                Console.Write("Please enter Note ID: ");
                stringInput = Console.ReadLine();
                Int32.TryParse(stringInput, out input);
            } while (string.IsNullOrEmpty(stringInput));

            var deletion = Note.DeleteNote(input);
            if (!deletion)
            {
                Console.WriteLine("Either not found or aborted. Back to menu\n");
                Menu();
            }
            else
            {
                Console.WriteLine("Successfully removed and updated Note.json\n");
                Menu();
            }
        }

        private static void WatchNote()
        {
            string input;
            int id;
            do
            {
                Console.Write("Please input note Id: ");
                input = Console.ReadLine();
                Int32.TryParse(input, out id);
            } 
            while (id < 0 || string.IsNullOrEmpty(input));

            var found = Note.GetNoteById(id);
            if (found == null)
            {
                Menu();
            }
            Note.ShowNote(found);
            Menu();
        }

        private static void SearchNote()
        {
            var note = new Note();
            var getNotes = Note.GetNotes();
            Console.Write("Please input filter-string: ");
            var input = Console.ReadLine();
            if (String.IsNullOrEmpty(input))
            {
                if(Note.PrintNotes(getNotes))
                    Menu();
            }

            var filtered = note.Filter(getNotes, input);
            if (filtered != null)
            {
                Console.WriteLine("\nFiltered:");
                Note.PrintNotes(filtered);
                Console.WriteLine('\n');
                
            }
            else
            {
                Console.WriteLine("\nNo notes found by your input!\n");
            }
            Menu();
        }

        private static void AddNote()
        {
            Console.WriteLine("Please enter Text of Note:");
            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Input cannot be empty! Returning to menu\n");
                Menu();
            }
            Note.AddNote(input);
            Console.WriteLine("Successfully Added note.\n");
            Menu();
        }
    }
}