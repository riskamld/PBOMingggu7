using System;
using System.Collections.Generic;
using System.Linq;


public abstract class Kendaraan
{
  
    private string NomorPlat { get; set; }
    private string Merk { get; set; }
    private string Warna { get; set; }
  
    private int DurasiSewa { get; set; } 

    
    public string GetNomorPlat() => NomorPlat;
    public string GetMerk() => Merk;
    public int GetDurasiSewa() => DurasiSewa;

    // Constructor
    public Kendaraan(string nomorPlat, string merk, string warna, int durasiSewa)
    {
        NomorPlat = nomorPlat;
        Merk = merk;
        Warna = warna;
        DurasiSewa = durasiSewa;
    }

    public abstract double HitungBiayaSewa(int telat);

    public double HitungBiayaSewa()
    {
        return HitungBiayaSewa(0); 
    }

    public virtual void DisplayInfo()
    {
        Console.WriteLine($"Nomor Plat: {NomorPlat}");
        Console.WriteLine($"Merk: {Merk}");
        Console.WriteLine($"Warna: {Warna}");
        Console.WriteLine($"Durasi Sewa (hari): {DurasiSewa}");
    }
}

public class Mobil : Kendaraan
{
    private const double HARGA_SEWA_PER_HARI = 300000;
    private const double BIAYA_ASURANSI = 100000;
    private const double DENDA_PER_HARI = 50000;

    public Mobil(string nomorPlat, string merk, string warna, int durasiSewa)
        : base(nomorPlat, merk, warna, durasiSewa) { }

    public override double HitungBiayaSewa(int telat)
    {
        double totalHargaSewaDasar = HARGA_SEWA_PER_HARI * GetDurasiSewa();
        double biayaDasar = totalHargaSewaDasar + BIAYA_ASURANSI;
        double denda = telat * DENDA_PER_HARI;
        return biayaDasar + denda;
    }
}


public class Motor : Kendaraan
{
    private const double HARGA_SEWA_PER_HARI = 100000;
    private const double DISKON_MOTOR = 50000;
    private const double DENDA_PER_HARI = 50000;

    public Motor(string nomorPlat, string merk, string warna, int durasiSewa)
        : base(nomorPlat, merk, warna, durasiSewa) { }

    public override double HitungBiayaSewa(int telat)
    {
        double totalHargaSewaDasar = HARGA_SEWA_PER_HARI * GetDurasiSewa();
        double biayaDasar = totalHargaSewaDasar - DISKON_MOTOR;
        if (biayaDasar < 0) biayaDasar = 0; 

        double denda = telat * DENDA_PER_HARI;
        return biayaDasar + denda;
    }
}


public class Bus : Kendaraan
{
    private const double HARGA_SEWA_PER_HARI = 500000;
    private const double BIAYA_SOPIR = 200000;
    private const double DENDA_PER_HARI = 50000;

    public Bus(string nomorPlat, string merk, string warna, int durasiSewa)
        : base(nomorPlat, merk, warna, durasiSewa) { }

    public override double HitungBiayaSewa(int telat)
    {
        double totalHargaSewaDasar = HARGA_SEWA_PER_HARI * GetDurasiSewa();
        double biayaDasar = totalHargaSewaDasar + BIAYA_SOPIR;
        double denda = telat * DENDA_PER_HARI;
        return biayaDasar + denda;
    }
}


public class Program
{
    // Helper method untuk memformat mata uang
    public static string FormatRupiah(double amount)
    {
        return string.Format("Rp{0:N0}", amount).Replace(",", ".");
    }

    public static void Main(string[] args)
    {
        List<Kendaraan> daftarKendaraan = new List<Kendaraan>();

        Console.Write("Masukkan jumlah kendaraan yang ingin disewa: ");
        if (!int.TryParse(Console.ReadLine(), out int jumlahKendaraan) || jumlahKendaraan < 0)
        {
            jumlahKendaraan = 0;
        }

        for (int i = 0; i < jumlahKendaraan; i++)
        {
            Console.WriteLine($"\nData Kendaraan ke-{(i + 1)}");
            string jenis, plat, merk, warna;
            int durasi = 0;

            do
            {
                Console.Write("Jenis Kendaraan (Mobil/Motor/Bus): ");
                jenis = Console.ReadLine()?.Trim() ?? string.Empty;
            } while (!new[] { "Mobil", "Motor", "Bus" }.Any(j => j.Equals(jenis, StringComparison.OrdinalIgnoreCase)));

            Console.Write("Nomor Plat: ");
            plat = Console.ReadLine() ?? string.Empty;
            Console.Write("Merk: ");
            merk = Console.ReadLine() ?? string.Empty;
            Console.Write("Warna: ");
            warna = Console.ReadLine() ?? string.Empty;

            while (durasi <= 0)
            {
                Console.Write("Durasi Sewa (hari): ");
                if (int.TryParse(Console.ReadLine(), out int d) && d > 0)
                {
                    durasi = d;
                }
                else
                {
                    Console.WriteLine("Input durasi tidak valid. Harap masukkan angka positif.");
                }
            }

            // Membuat objek dan menambahkan ke list
            Kendaraan k;
            if (jenis.Equals("Mobil", StringComparison.OrdinalIgnoreCase))
            {
                k = new Mobil(plat, merk, warna, durasi);
            }
            else if (jenis.Equals("Motor", StringComparison.OrdinalIgnoreCase))
            {
                k = new Motor(plat, merk, warna, durasi);
            }
            else 
            {
                k = new Bus(plat, merk, warna, durasi);
            }
            daftarKendaraan.Add(k);
        }

        Console.WriteLine("\nHASIL PERHITUNGAN BIAYA SEWA");
        int[] hariTelat = new int[daftarKendaraan.Count];

        for (int i = 0; i < daftarKendaraan.Count; i++)
        {
            Kendaraan k = daftarKendaraan[i];
            int telat = 0;

        
            Console.Write($"Masukkan jumlah hari telat untuk {k.GetMerk()} ({k.GetNomorPlat()}), tekan Enter jika tidak telat: ");
            string inputTelat = Console.ReadLine()?.Trim() ?? string.Empty;

            if (!string.IsNullOrEmpty(inputTelat))
            {
                if (int.TryParse(inputTelat, out int t) && t > 0)
                {
                    telat = t;
                }
                // Jika input tidak valid atau angka negatif, telat tetap 0
            }
            hariTelat[i] = telat;
        }

        for (int i = 0; i < daftarKendaraan.Count; i++)
        {
            Kendaraan k = daftarKendaraan[i];
            int telat = hariTelat[i];
            double biayaTanpaTelat = k.HitungBiayaSewa();
            double biayaDenganTelat = k.HitungBiayaSewa(telat);

            Console.WriteLine($"\nKendaraan: {k.GetMerk()} – {k.GetNomorPlat()}");
            Console.WriteLine($"Biaya sewa (tanpa telat): {FormatRupiah(biayaTanpaTelat)}");
            Console.WriteLine($"Biaya sewa (telat {telat} hari): {FormatRupiah(biayaDenganTelat)}");
        }
    }
}