using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Task_3
{
    internal class Note : INote
    {
        private const string _file = "Notes.json";

        public Note(int id, string title, string text, DateTime createdOn)
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

        public List<Note> Filter(List<Note> notes, string filter)
        {
            if (IsEmpty(notes))
                return null;
            var filteredNotes = new List<Note>();
            foreach (var note in notes)
            {
                if (note.Id.ToString().Contains(filter) || note.Title.Contains(filter) ||
                    note.Text.Contains(filter) || note.CreatedOn.ToString().Contains(filter))
                {
                    filteredNotes.Add(note);
                }
            }
            return filteredNotes.Count < 1 ? null : filteredNotes;
        }

        private static bool IsExist()
        {
            if (File.Exists("Notes.json")) return true;
            Console.WriteLine("File Notes.json not found. Creating...");
            using var fs = File.Create(_file);
            fs.Close();
            return true;
        }
        
        public static bool PrintNotes(IEnumerable<Note> notesList)
        {
            if (IsEmpty(notesList?.ToList()))
                return true;
            foreach (var note in notesList)
            {
                Console.WriteLine(note.ToString());
            }

            return false;
        }
        public static void AddNote(string text)
        {
            text = text.Trim();
            string result;
            IsExist();
            if (File.ReadAllText(_file).Length == 0)
            {
                result = "[";
                var note = NewNote(text, 0);
                result += JsonConvert.SerializeObject(note);
                result += "]";
            }
            else
            {
                var deserialize = JsonConvert.DeserializeObject<List<Note>>(File.ReadAllText(_file));
                var newId = deserialize.Max(x => x.Id)+1;
                var note = NewNote(text, newId);
                deserialize.Add(note);
                result = JsonConvert.SerializeObject(deserialize,Formatting.Indented);
            }
            File.WriteAllText(_file,result);
        }

        private static void UpdateNoteJson(int id)
        {
            var deserialize = JsonConvert.DeserializeObject<List<Note>>(File.ReadAllText(_file));
            deserialize.RemoveAt(id);
            if (deserialize.Count == 0)
            {
                File.WriteAllText(_file,null);
            }
            else
            {
                var json = JsonConvert.SerializeObject(deserialize,Formatting.Indented);
                File.WriteAllText(_file,json);
            }
        }

        public static bool DeleteNote(int id)
        {
            var notes = GetNotes();
            if (IsEmpty(notes))
            {
                return false;
            }

            if (id < 0 || id > notes.Count-1) return false;
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
            var date = DateTime.Now;
            if (text.Length < 32) return new Note(id, text, text, date);
            var title = text.Substring(0, 32);
            return new Note(id, title, text, date);
        }

        public static List<Note> GetNotes()
        {
            IsExist();
            var deserialize = JsonConvert.DeserializeObject<List<Note>>(File.ReadAllText(_file));
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
            if (id < 0 || id > notes.Count())
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