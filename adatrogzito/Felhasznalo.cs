using System;
using System.ComponentModel.DataAnnotations;

namespace adatrogzito
{
    internal class Felhasznalo
    {
        [Required(ErrorMessage = "Név is required")]
        [MinLength(3, ErrorMessage = "Név must be at least 3 characters long")]
        public string Nev { get; set; }

        [Required(ErrorMessage = "Kor is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Kor must be a positive integer")]
        public int Kor { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefonszám is required")]
        [MinLength(8, ErrorMessage = "Telefonszám must be at least 8 characters long")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Telefonszám must contain only numbers")]
        public string Telefonszam { get; set; }

        [Required(ErrorMessage = "Cím is required")]
        public string Lakcim { get; set; }

        [Required(ErrorMessage = "Nem is required")]
        public string Neme { get; set; }

        public string Megjegyzes { get; set; }

        public Felhasznalo(string nev, int kor, string email, string telefonszam, string lakcim, string neme, string megjegyzes = null)
        {
            Nev = nev;
            Kor = kor;
            Email = email;
            Telefonszam = telefonszam;
            Lakcim = lakcim;
            Neme = neme;
            Megjegyzes = megjegyzes;

            Validate();
        }

        private void Validate()
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(this, null, null);
            if (!Validator.TryValidateObject(this, context, validationResults, true))
            {
                string errors = string.Join("\n", validationResults.Select(vr => vr.ErrorMessage));
                throw new ValidationException(errors);
            }
        }
    }
}