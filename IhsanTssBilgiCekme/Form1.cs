using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Web.WebView2.WinForms;
using System.Reflection;



// Sıkıntılar:  excele yazdırma
//              lastIndexOf meselesi (konyaaltı/ankara)
namespace IhsanTssBilgiCekme
{
    public partial class Form1 : Form
    {
        private HospitalInfo hpInfo;
        private int sehirSayisiIndex = 3;
        private string hastaneAdi = "";
        private string tempil = "";
        private string ilce = "";
        private string AcikAdres = "";
        private string sigortaSirketi = "";
        private string antlasmaliKurumSayisi = "";
        private int currentRow=3; // current column tutmaya gerek yok o işlem sırasında seçilecek
        private int birSonrakiGirilecekSehirIndexi = 4;
        private int birSonrakiGirilecekHastaneIndexi = 0;
        private int sayfadaGosterilenHastaneSayisi = 0;
        private int sayfaIndexi = 1; //ilk başta değeri 7, bir kere tıklandıktan sonra 8 olacak.
        private bool birSonrakiSayfayaGecildi = false; //hastanelerin olduğu sayfada, antlaşmalı kurum sayısına göre bir sonraki sayfaya geçildi mi geçilmedi mi buna bunu kontrol eder
        private bool ilkSayfa = true;
        private List<Kurumlar> kurum = new List<Kurumlar>();
        private Kurumlar kr;
        private static int row = 2;
        private bool sehirDegistir = true;
        private bool kurumDegistir = true;
        private bool bilgileriAl = true;
        private bool geriGit = true;
        private int sayfaSayisi = 0;
        WebView2 webView = new WebView2();
        private int rowNum = 2;
        private int dosyaAdiIndex = 0;
        public struct Kurumlar
        {
            public string hastaneAdi;
            public string il;
            public string ilce;
            public string acikAdres;
            public List<string> antlasmaliSigortaSirketleri;
            public List<string>  antlasmaliSigortaNetworkler;

        }

        public Form1()
        {
            InitializeComponent();
            this.Resize += new System.EventHandler(this.Form_Resize);
            webView21.NavigationStarting += EnsureHttps;

        }
    
        private void Form_Resize(object sender, EventArgs e)
        {
            webView21.Size = this.ClientSize - new System.Drawing.Size(webView21.Location);
           // sayfa1.Left = this.ClientSize.Width - sayfa1.Width;
           // addressBar.Width = sayfa1.Left - addressBar.Left;
        }



        //https://www.tamamlayicisaglik.com/anlasmali-saglik-kurumlari/sonuclar/hastane/30001?cityId=34&productTypeId=1
        private string url_p1 = "https://www.tamamlayicisaglik.com/anlasmali-saglik-kurumlari/sonuclar/hastane/";
        private string url_p2 = "?cityId=34&productTypeId=1";
        private async void DoItWithNavigate()
        {
            int k = 0;
            int i = 0;

            //WriteStringToExcelFile("Mahalle_Listesi_" + dosyaAdiIndex + ".xls");
            //string v = AdrestenSehirIsmiAl("\r\nMal Pazarı Mevkii G-CITY Avm Yanı 28100 / Merkez/ asdasd");

            using (var streamReader = new StreamReader("link.txt"))
            {
                // Read the file line by line
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    Console.WriteLine(k);
                    webView21.CoreWebView2.Navigate(line);
                    await Task.Delay(600);
                    await HastaneBilgileriniCek();
                    await Task.Delay(200);

                    if (k >= 779)
                    {
                        WriteStringToExcelFile("res" + dosyaAdiIndex + ".xls");   //her 500 kayıtta bir dosya 
                        k = 0;
                        return;
                    }
                    k++;
                }
                //WriteStringToExcelFile("output" + dosyaAdiIndex + ".xls");
            }
            //for (int i = 16001; i <= 33001; i++)
            //{
            //    Console.Write(i+" - ");
            //    Console.WriteLine(k);
            //    webView21.CoreWebView2.Navigate(url_p1 + i+url_p2);
            //    await Task.Delay(600);
            //    await HastaneBilgileriniCek();
            //    await Task.Delay(200);

            //    if (k >= 1000) {
            //        WriteStringToExcelFile("Mahalle_Listesi_" + dosyaAdiIndex + ".xls");   //her 500 kayıtta bir dosya 
            //        k = 0;
            //    }
            //    k++;
            //}
            //webView21.CoreWebView2.Navigate(url + "151");
            //await HastaneBilgileriniCek();
            //await Task.Delay(600);
            //webView21.CoreWebView2.Navigate(url + "50000");
            //await HastaneBilgileriniCek();
            //await Task.Delay(600);
            //webView21.CoreWebView2.Navigate(url + "150");
            //await HastaneBilgileriniCek();
            Console.WriteLine("END");
        }

/// <summary>
/// halil
/// </summary>
/// <returns></returns>
        private async Task HastaneBilgileriniCek()
        {

            //string code = @"
            //        var innerText = $(""#app > div.error-404 > div.container.section-with-icons > div > div > div.col-md-6 > h1"").innerText
            //        innerText
            //";
            //var innerText = await webView21.CoreWebView2.ExecuteScriptAsync(code);
            //if (innerText != "null") return;

            string code = @"
                    var antlasmaliNetworkler = $(""div.description div"").text().trim();                               //  anlaşmalı networkler
                    antlasmaliNetworkler = antlasmaliNetworkler.split(""\n                    "")
                    antlasmaliNetworkler = antlasmaliNetworkler.filter(elm => elm)
                    antlasmaliNetworkler.pop();
                    antlasmaliNetworkler
            ";
            var antlasmaliNetworkler = await webView21.CoreWebView2.ExecuteScriptAsync(code);


            code = @"
                    var sigortaSirketleri = [];                                                                        //  sigorta şirketleri
                    $('.company-list .title').each(function() {
                      if($(this).text() != ""Anlaşmalı networkler"")  
                          sigortaSirketleri.push($(this).text());
                    });
                    sigortaSirketleri
            ";
            var sigortaSirketleri = await webView21.CoreWebView2.ExecuteScriptAsync(code);
            code = @"
                    var ilce = jQuery("".type-box"")[0].children[1].innerText    
                    var acikAdres = jQuery("".d-md-flex.d-block.align-items-md-center.align-items-baseline"").children(0)[0].children[1].innerText
                    var hastaneAdi = jQuery("".box"")[0].children[0].children[2].innerText;
                    var il = jQuery("".d-md-flex.d-block.align-items-md-center.align-items-baseline"").children(0)[0].children[1].innerText.split('/')[1]
                    var temp = ilce + '|' + acikAdres + '|' + hastaneAdi
                    temp
            ";
            var ilceAcikAdresHastaneAdi = await webView21.CoreWebView2.ExecuteScriptAsync(code);
            if (ilceAcikAdresHastaneAdi == "null")
            {
                //await HastaneBilgileriniCek();
                return;

            }
            ilceAcikAdresHastaneAdi = JsonConvert.DeserializeObject(ilceAcikAdresHastaneAdi).ToString();

            var bilgiler = ilceAcikAdresHastaneAdi.Split('|');


            //await Task.Delay(200);

            var sigortaSirketleriArr = sigortaSirketleri.Split('"');
            var antlasmaliNetworklerArr = antlasmaliNetworkler.Split('"');
            if (sigortaSirketleriArr[0] == "[]")
                return;
            List<string> sigortaSirketleriList = new List<string>();
            List<string> antlasmaliNetworklerList = new List<string>();

            for (int i = 1; i < antlasmaliNetworklerArr.Length; i += 2)
            {
                sigortaSirketleriList.Add(sigortaSirketleriArr[i]);
                antlasmaliNetworklerList.Add(antlasmaliNetworklerArr[i]);
            }

            ///Console.WriteLine("sigortaSirketleriArr[2]: " + sigortaSirketleriArr[1]);
            kr = new Kurumlar();
            string remove = " Anlaşmalı Sigorta Şirketleri";
            //kr.hastaneAdi = bilgiler[2].Remove(bilgiler[2].IndexOf(remove),remove.Length);
            kr.hastaneAdi = bilgiler[2];
            kr.il = bilgiler[1].Substring(bilgiler.Length / 2);//AdrestenSehirIsmiAl(bilgiler[1].Substring(bilgiler.Length/2)); //şehir ismine son yarısında bakılır
            kr.ilce = bilgiler[0];
            //kr.acikAdres = bilgiler[1];
            kr.antlasmaliSigortaNetworkler = antlasmaliNetworklerList;
            kr.antlasmaliSigortaSirketleri = sigortaSirketleriList;

            kurum.Add(kr);

        }

        private string AdrestenSehirIsmiAl(string txt)
        {
            List<String> tempL = new List<string>();
            int index = -1;
            int rIndex = 0;
            int r = 0;
            bool buldu = false;

            txt = txt.ToLower();
            for (int i = 0; i < turkishCities.Count; i++)
                if (txt.Contains(turkishCities[i].ToLower()))
                {
                    tempL.Add(turkishCities[i]);
                    buldu = true;
                    r = i;
                    //return turkishCities[i];
                }
            if (tempL.Count == 1) return turkishCities[r];
            for (int i = 0; i < tempL.Count; i++)   //izmir Bulvarı/Denizli gibi adresler için. sondakini almaya yarar
            {
                var v = txt.LastIndexOf(tempL[i]);
                if (v > index)
                {
                    index = v;
                    rIndex = i;
                }
            }
            if (buldu)
                return turkishCities[turkishCities.IndexOf(tempL[rIndex])];
            return "";
        }


        //şehir
        private void sayfa1_Click(object sender, EventArgs e) //sayfa1_click
        {
            //BirSonrakiSehreGir();
            DoItWithNavigate();

        }


        /// <summary>
        /// İkinci sayfadaki fonksiyonları çalıştırır
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sayfa2_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 3. sayfa fonksiyonlarını çalıştırır
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sayfa3_Click(object sender, EventArgs e)
        {
        }

        private async void kontrol_Click(object sender, EventArgs e)
        {
            //WriteStringToExcelFile(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Result.xlsx", kurum);
            // WriteToExcel("Altınkoza Hastanesi Anlaşmalı Sigorta Şirketleri\r\nAdana\r\nYüreğir\r\n\r\nAK Sigorta - T1 Network, T2 Network\r\nAXA Sigorta -     Sağlığım Tamam Nw., Sağlığım Tamam B Nw., AXA Sağlığım Tamam Tutumlu Sigortası\r\nAllianz Sigorta -     Turuncu Network, Turkuaz Network\r\nAnadolu Sigorta -     B network, A network\r\nAveon Sigorta -     Standart Network\r\nBupaAcıbadem Sigorta -     T1 Network, T2 Network\r\nDoğa Sigorta -     Tamamlayıcı Sağlık Network\r\nEureko Sigorta -     Tamamlayıcı Sağlık Network\r\nGroupama Sigorta -     Tamamlayıcı Sağlık Network\r\nHDI Sigorta -     Tamamlayıcı sağlık network\r\nKatılım Sağlık Sigorta -     Tamamlayıcı Sağlık Network\r\nMagdeburger Sigorta -     Tamamlayıcı Sağlık Network\r\nMapfre Sigorta -     Tamamlayıcı Sağlık Network\r\nNN Sigorta -     B Network, A Network\r\nQuick Sigorta -     Lacivert Network, Yeşil Network, Pembe Network\r\nRAY Sigorta -     Tamamlayıcı Sağlık Network\r\nSompo Sigorta -     A Network, B Network\r\nTürk Nippon Sigorta -     Tamamlayıcı Sağlık Network\r\nTürkiye Sigorta -     TSS ANADOLU NETWORK, TSS GÜMÜŞ NETWORK, TSS ALTIN NETWORK");
            WriteStringToExcelFile("Mahalle_Listesi_"+ dosyaAdiIndex + ".xls");
        }
        public void WriteToExcel(string text)
        {
            string filePath = "filename.xlsx"; // Replace with your desired file path

            Excel.Application excel = new Excel.Application();
            Workbook workbook = null;

            try
            {
                // Check if the file exists
                if (File.Exists(filePath))
                {
                    // If it exists, open it and get the last used row
                    workbook = excel.Workbooks.Open(filePath, false);
                    Worksheet worksheet = (Worksheet)workbook.Worksheets[1];
                    int lastRow = worksheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell).Row;

                    // Write the text to the next row
                    string[] lines = text.Split('\n');
                    int column = worksheet.Cells[lastRow, worksheet.Columns.Count].End[XlDirection.xlToLeft].Column + 1;
                    foreach (string line in lines)
                    {
                        worksheet.Cells[lastRow, column] = line.Trim();
                        column++;
                    }

                    // Save and close the workbook
                    workbook.Save();
                    workbook.Close();
                }
                else
                {
                    // If it does not exist, create a new workbook and write the text to the first row
                    workbook = excel.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                    Worksheet worksheet = (Worksheet)workbook.Worksheets[1];

                    string[] lines = text.Split('\n');
                    int row = 1;
                    int column = 1;
                    foreach (string line in lines)
                    {
                        string[] columns = line.Split('\t');
                        foreach (string col in columns)
                        {
                            worksheet.Cells[row, column] = col.Trim();
                            column++;
                        }
                        row++;
                        column = 1;
                    }

                    // Save and close the workbook
                    workbook.SaveAs(filePath);
                    workbook.Close();
                }
            }
            finally
            {
                // Release COM objects
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            }
        }


        private static int SigortaSirketiRow(string sigortaSirketi)
        {
            sigortaSirketi = sigortaSirketi.ToLower();

            if (sigortaSirketi.Contains("bupaacıbadem")) return 47;
            else if (sigortaSirketi.Contains("acıbadem")) return 5;
            else if (sigortaSirketi.Contains("acnturk")) return 6;
            else if (sigortaSirketi.Contains("ak")) return 7;
            else if (sigortaSirketi.Contains("allianz")) return 8;
            else if (sigortaSirketi.Contains("anadolu")) return 10;
            else if (sigortaSirketi.Contains("ana")) return 9;
            else if (sigortaSirketi.Contains("ankara")) return 11;
            else if (sigortaSirketi.Contains("arex")) return 12;
            else if (sigortaSirketi.Contains("atlas")) return 13;
            else if (sigortaSirketi.Contains("aveon")) return 14;
            else if (sigortaSirketi.Contains("axa")) return 15;
            else if (sigortaSirketi.Contains("bereket")) return 16;
            else if (sigortaSirketi.Contains("corpus")) return 17;
            else if (sigortaSirketi.Contains("doğa")) return 18;
            else if (sigortaSirketi.Contains("ethica")) return 19;
            else if (sigortaSirketi.Contains("eureko")) return 20;
            else if (sigortaSirketi.Contains("generalli")) return 21;
            else if (sigortaSirketi.Contains("gri")) return 22;
            else if (sigortaSirketi.Contains("groupama")) return 23;
            else if (sigortaSirketi.Contains("gulf")) return 24;
            else if (sigortaSirketi.Contains("hdı")) return 25;
            else if (sigortaSirketi.Contains("hepiyi")) return 26;
            else if (sigortaSirketi.Contains("koru")) return 27;
            else if (sigortaSirketi.Contains("magdeburger")) return 28;
            else if (sigortaSirketi.Contains("mapfre")) return 29;
            else if (sigortaSirketi.Contains("neova")) return 30;
            else if (sigortaSirketi.Contains("orient")) return 31;
            else if (sigortaSirketi.Contains("prive")) return 32;
            else if (sigortaSirketi.Contains("quick")) return 33;
            else if (sigortaSirketi.Contains("ray")) return 34;
            else if (sigortaSirketi.Contains("sompo")) return 35;
            else if (sigortaSirketi.Contains("şeker")) return 36;
            else if (sigortaSirketi.Contains("tmt")) return 37;
            else if (sigortaSirketi.Contains("türkiye")) return 38;
            else if (sigortaSirketi.Contains("türk nippon")) return 39;
            else if (sigortaSirketi.Contains("unico")) return 40;
            else if (sigortaSirketi.Contains("zurich")) return 41;
            else if (sigortaSirketi.Contains("katılım sağlık")) return 42;
            else if (sigortaSirketi.Contains("nn sigorta")) return 43;
            else if (sigortaSirketi.Contains("aegon")) return 44;
            else if (sigortaSirketi.Contains("demir")) return 45;
            else if (sigortaSirketi.Contains("fiba")) return 46;
            else return -1;

        }

        public void writeTxt()
        {
            using (StreamWriter sw = File.AppendText("SavedList.txt")) 
            { 
                foreach (var s in kurum)
                {
                    sw.WriteLine("# " + s.hastaneAdi);
                    sw.WriteLine(s.il);
                    sw.WriteLine(s.ilce);
                    sw.WriteLine(s.acikAdres);
                    for (int i = 0; i < s.antlasmaliSigortaNetworkler.Count; i++)
                    {
                        sw.WriteLine(s.antlasmaliSigortaSirketleri[i] + " - " + s.antlasmaliSigortaNetworkler[i]);
                    }
                    sw.WriteLine("--------------------------------------------------------------------------");
                }
            }
            kurum.Clear();
        }

        void EnsureHttps(object sender, CoreWebView2NavigationStartingEventArgs args)
        {
            String uri = args.Uri;
            if (!uri.StartsWith("https://"))
            {
                webView21.CoreWebView2.ExecuteScriptAsync($"alert('{uri} is not safe, try an https link')");
                args.Cancel = true;
            }
        }

        /// <summary>
        /// yazım sırası: Hastane Adi - İli - ilçesi - AçıkAdres - hastane indexi
        /// </summary>
        /// <param name="kr"></para
        public void WriteStringToExcelFile(string fileName)
        {

            Excel.Application excelApp = null;
            Excel.Workbook excelWorkbook = null;
            Excel.Worksheet excelWorksheet = null;

            try
            {
                excelApp = new Excel.Application();
                excelApp.Visible = false; // Don't show the Excel application
                excelWorkbook = OpenOrCreateExcelWorkbook(excelApp, fileName);
                excelWorksheet = (Excel.Worksheet)excelWorkbook.ActiveSheet;
                int col = 2;
                int row = rowNum;

                //Excel.Range cell = (Excel.Range)excelWorksheet.Cells[col][row];
                //     cell.Value = "1";
                //cell = (Excel.Range)excelWorksheet.Cells[row+1][col];
                //cell.Value = "2";
                //cell = (Excel.Range)excelWorksheet.Cells[row][col+1];
                //cell.Value = "3";

                // Write the data to the specified row and column
                foreach (Kurumlar item in kurum)
                {
                    Excel.Range cell = (Excel.Range)excelWorksheet.Cells[col][row];
                    cell.Value = item.hastaneAdi;
                    cell = (Excel.Range)excelWorksheet.Cells[col + 1][row];
                    cell.Value = item.il;
                    cell = (Excel.Range)excelWorksheet.Cells[col + 2][row];
                    cell.Value = item.ilce;
                    int length = item.antlasmaliSigortaNetworkler.Count();
                    for (int i = 0; i < length; i++)
                    {
                        var v = item.antlasmaliSigortaSirketleri[i];
                        var coll = SigortaSirketiRow(item.antlasmaliSigortaSirketleri[i]);
                        if (coll == -1) continue;
                        cell = (Excel.Range)excelWorksheet.Cells[coll][row];
                        cell.Value = item.antlasmaliSigortaNetworkler[i];
                    }
                    row++;
                }

                // Save and close the workbook
                object missingValue = System.Reflection.Missing.Value;
                excelWorkbook.Save();
                excelWorkbook.Close(false, missingValue, missingValue);
                //excelWorkbook.Close();
                excelApp.Quit();
            }
            finally
            {
                // Clean up the Excel objects
                if (excelWorksheet != null) Marshal.ReleaseComObject(excelWorksheet);
                if (excelWorkbook != null) Marshal.ReleaseComObject(excelWorkbook);
                if (excelApp != null) Marshal.ReleaseComObject(excelApp);
            }
            rowNum = rowNum + kurum.Count;
            kurum.Clear();
            dosyaAdiIndex++;
        }

        private static Excel.Workbook OpenOrCreateExcelWorkbook(Excel.Application excelApp, string fileName)
        {
            Excel.Workbook excelWorkbook = null;

            // Check if the file already exists
            if (File.Exists(fileName))
            {
                // If it exists, open the workbook
                excelWorkbook = excelApp.Workbooks.Open(fileName);
            }
            else
            {
                // If it doesn't exist, create a new workbook
                excelWorkbook = excelApp.Workbooks.Add();
                excelWorkbook.SaveAs(fileName);
            }

            return excelWorkbook;
        }

      
        List<string> turkishCities = new List<string>
        {
            "Adana",
            "Adıyaman",
            "Afyonkarahisar",
            "Ağrı",
            "Amasya",
            "Ankara",
            "Antalya",
            "Artvin",
            "Aydın",
            "Balıkesir",
            "Bilecik",
            "Bingöl",
            "Bitlis",
            "Bolu",
            "Burdur",
            "Bursa",
            "Çanakkale",
            "Çankırı",
            "Çorum",
            "Denizli",
            "Diyarbakır",
            "Edirne",
            "Elazığ",
            "Erzincan",
            "Erzurum",
            "Eskişehir",
            "Gaziantep",
            "Giresun",
            "Gümüşhane",
            "Hakkari",
            "Hatay",
            "Isparta",
            "Mersin",
            "İstanbul",
            "İzmir",
            "Kars",
            "Kastamonu",
            "Kayseri",
            "Kırklareli",
            "Kırşehir",
            "Kocaeli",
            "Konya",
            "Kütahya",
            "Malatya",
            "Manisa",
            "Kahramanmaraş",
            "Mardin",
            "Muğla",
            "Muş",
            "Nevşehir",
            "Niğde",
            "Ordu",
            "Rize",
            "Sakarya",
            "Samsun",
            "Siirt",
            "Sinop",
            "Sivas",
            "Tekirdağ",
            "Tokat",
            "Trabzon",
            "Tunceli",
            "Şanlıurfa",
            "Uşak",
            "Van",
            "Yozgat",
            "Zonguldak",
            "Aksaray",
            "Bayburt",
            "Karaman",
            "Kırıkkale",
            "Batman",
            "Şırnak",
            "Bartın",
            "Ardahan",
            "Iğdır",
            "Yalova",
            "Karabük",
            "Kilis",
            "Osmaniye",
            "Düzce"
        };
    }

}

