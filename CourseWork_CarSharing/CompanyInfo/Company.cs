using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace CourseWork_CarSharing.CompanyInfo
{
    public class Company
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; } // Номер расчетного счета компании.
        public string CorrespondentAccount { get; set; } // Корреспондентский счет банка (если требуется).
        public string BIK { get; set; } // БИК (Банковский идентификационный код) банка.
        public string INN { get; set; } // ИНН (Идентификационный номер налогоплательщика) компании.


        public void GenerateOrganizationDocument(string outputPath)
        {
            // Создание документа Word
            using (WordprocessingDocument document = WordprocessingDocument.Create(outputPath, WordprocessingDocumentType.Document))
            {
                // Добавление основного раздела документа
                MainDocumentPart mainPart = document.AddMainDocumentPart();
                mainPart.Document = new Document();

                // Создание абзаца для каждого поля данных
                Paragraph nameParagraph = CreateParagraph("Name:", Name);
                Paragraph addressParagraph = CreateParagraph("Address:", Address);
                Paragraph phoneParagraph = CreateParagraph("Phone:", Phone);
                Paragraph emailParagraph = CreateParagraph("Email:", Email);
                Paragraph bankNameParagraph = CreateParagraph("BankName:", BankName);
                Paragraph accountNumberParagraph = CreateParagraph("AccountNumber:", AccountNumber);
                Paragraph correspondentAccountParagraph = CreateParagraph("CorrespondentAccount:", CorrespondentAccount);
                Paragraph BIKParagraph = CreateParagraph("BIK:", BIK);
                Paragraph INNParagraph = CreateParagraph("INN:", INN);

                // Добавление абзацев в раздел документа
                Body body = mainPart.Document.AppendChild(new Body());
                body.AppendChild(nameParagraph);
                body.AppendChild(addressParagraph);
                body.AppendChild(phoneParagraph);
                body.AppendChild(emailParagraph);
                body.AppendChild(bankNameParagraph);
                body.AppendChild(accountNumberParagraph);
                body.AppendChild(correspondentAccountParagraph);
                body.AppendChild(BIKParagraph);
                body.AppendChild(INNParagraph);
            }
        }

        private Paragraph CreateParagraph(string label, string value)
        {
            // Создание абзаца с меткой и значением
            Paragraph paragraph = new Paragraph();

            // Добавление метки
            RunProperties labelRunProperties = new RunProperties(new Bold());
            Run labelRun = new Run(new Text(label));
            labelRun.RunProperties = labelRunProperties;
            paragraph.AppendChild(labelRun);

            // Добавление значения
            Run valueRun = new Run(new Text(value));
            paragraph.AppendChild(valueRun);

            return paragraph;
        }
    }
}
