using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Task_3
{
    internal class Note : INote
    {
        private const string File = "Notes.json";

        private Note(int id, string title, string text, DateTime createdOn)
        {
            Id = id;
            Title = title;
            Text = text;
            CreatedOn = createdOn;
        }

        public Note()
        {
            
        }

        public int Id { get; private set; }

        public string Title { get; private set; }


        public string Text { get; private set; }

        public DateTime? CreatedOn { get; private set; }

        private static bool IsEmpty(List<Note> note)
        {
            if (note != null) return false;
            Console.WriteLine("Note.json is empty!");
            return true;

        }

        public static List<Note> Filter(List<Note> notes, string filter)
        {
            if (IsEmpty(notes))
                return null;
            var filteredNotes = notes.Where(note => note.Id.ToString().Contains(filter) || note.Title.Contains(filter) || note.Text.Contains(filter) || note.CreatedOn.ToString().Contains(filter)).ToList();
            return filteredNotes.Count < 1 ? null : filteredNotes;
        }

        private static void IsExist()
        {
            if (System.IO.File.Exists("Notes.json")) return;
            Console.WriteLine("File Notes.json not found. Creating...");
            using var fs = System.IO.File.Create(File);
            fs.Close();
        }
        
        public static bool PrintNotes(IEnumerable<Note> notesList)
        {
            var enumerable = notesList.ToList();
            if (IsEmpty(enumerable?.ToList()))
                return true;
            foreach (var note in enumerable)
            {
                Console.WriteLine(note.ToString());
            }

            return false;
        }
        public static void AddNote(string text)
        {
            text = text.Trim();
            string result = null;
            IsExist();
            if (System.IO.File.ReadAllText(File).Length == 0)
            {
                result = "[";
                var note = NewNote(text, 0);
                result += JsonConvert.SerializeObject(note);
                result += "]";
            }
            else
            {
                try
                {
                    var deserialize = JsonConvert.DeserializeObject<List<Note>>(System.IO.File.ReadAllText(File));
                    var newId = deserialize.Max(x => x.Id)+1;
                    var note = NewNote(text, newId);
                    deserialize.Add(note);
                    result = JsonConvert.SerializeObject(deserialize,Formatting.Indented);
                }
                catch
                {
                    Console.WriteLine($"Note.json file is corrupted! Please check it at {Path.GetFullPath(File)}");
                    Environment.Exit(-1);
                }
                
            }
            System.IO.File.WriteAllText(File,result);
        }

        private static void UpdateNoteJson(int id)
        {
            var deserialize = JsonConvert.DeserializeObject<List<Note>>(System.IO.File.ReadAllText(File));
            var toDelete = deserialize.FindIndex(x => x.Id.Equals(id));
            
            deserialize.RemoveAt(toDelete);
            if (deserialize.Count == 0)
            {
                System.IO.File.WriteAllText(File,null);
            }
            else
            {
                var json = JsonConvert.SerializeObject(deserialize,Formatting.Indented);
                System.IO.File.WriteAllText(File,json);
            }
        }

        public static bool DeleteNote(int id)
        {
            var notes = GetNotes();
            if (IsEmpty(notes))
            {
                return false;
            }

            if (notes.All(x => x.Id != id)) return false;
            Console.WriteLine($"Are you sure want to delete this note:");
            ShowNote(GetNoteById(id));
            Console.Write("Please write Y or N to continue: ");
            var input = Console.ReadLine();
            if (!input.ToLower().Equals("y")) return false;
            UpdateNoteJson(id);
            return true;

        }

        private static Note NewNote(string text, int id)
        {
            var date = DateTime.UtcNow;
            if (text.Length < 32) return new Note(id, text, text, date);
            var title = text.Substring(0, 32);
            return new Note(id, title, text, date);
        }

        public static List<Note> GetNotes()
        {
            IsExist();
            var deserialize = JsonConvert.DeserializeObject<List<Note>>(System.IO.File.ReadAllText(File));
            var notes = deserialize?.ToList();
            return notes;
        }

        public override string ToString()
        {
            return $"{Id} , {Title}, {CreatedOn}";
        }

        public static Note GetNoteById(int id)
        {
            var notes = GetNotes();
            if (IsEmpty(notes?.ToList()))
            {
                return null;
            }
            //if (id < 0 || id > notes.Count-1)
            if (!notes.Any(x=>x.Id == id))
            {
                Console.WriteLine("\nNot found such Note\n");
                return null;
            }
            foreach (var note in notes)
            {
                if (note.Id.Equals(id))
                {
                    return note;
                }
            }
            return null;
        }

        public static void ShowNote(Note note)
        {
            Console.WriteLine($"\nID: {note.Id},\nTitle:{note.Title},\nText:{note.Text},\nCreatedOn:{note.CreatedOn}\n");
        }
    }
}