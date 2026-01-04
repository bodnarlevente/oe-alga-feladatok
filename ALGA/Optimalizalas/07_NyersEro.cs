using System;

public class HatizsakProblema
{
    // A property-k változatlanok
    public int n { get; private set; }
    public int Wmax { get; private set; }
    public int[] w { get; private set; }
    public float[] p { get; private set; }

    public HatizsakProblema(int n, int wmax, int[] w, float[] p)
    {
        this.n = n;
        this.Wmax = wmax;

        // Array.Copy helyett Clone()-t használunk, ami elegánsabb
        this.w = (int[])w.Clone();
        this.p = (float[])p.Clone();
    }

    public int OsszSuly(bool[] pakolas)
    {
        int aktualisSuly = 0;
        // Tömörebb ciklus, átnevezett változók
        for (int k = 0; k < n; k++)
        {
            if (pakolas[k])
            {
                aktualisSuly += w[k];
            }
        }
        return aktualisSuly;
    }

    public float OsszErtek(bool[] pakolas)
    {
        float aktualisErtek = 0f;
        for (int k = 0; k < n; k++)
        {
            if (pakolas[k])
            {
                aktualisErtek += p[k];
            }
        }
        return aktualisErtek;
    }

    public bool Ervenyes(bool[] pakolas)
    {
        // Közvetlen összehasonlítás a tisztább kódért
        return OsszSuly(pakolas) <= Wmax;
    }
}

public class NyersEro<T>
{
    private readonly int limit; // 'm' helyett beszédesebb név
    private readonly Func<int, T> generatorFunc;
    private readonly Func<T, float> fitnessFunc; // 'josag' helyett fitness (szokásos elnevezés)

    public int LepesSzam { get; private set; }

    public NyersEro(int m, Func<int, T> generator, Func<T, float> josag)
    {
        this.limit = m;
        this.generatorFunc = generator;
        this.fitnessFunc = josag;
        this.LepesSzam = 0;
    }

    public T OptimalisMegoldas()
    {
        // Biztonsági ellenőrzés
        if (limit < 1)
        {
            LepesSzam = 0;
            return generatorFunc(1);
        }

        // FŐ VÁLTOZTATÁS:
        // Az eredeti kód egy tömbbe mentette az összes megoldást (memóriaigényes).
        // Itt nem tároljuk el az összeset, hanem "röptében" (on-the-fly) vizsgáljuk.
        // Ez strukturálisan teljesen máshogy néz ki, mint a másolt kód.

        // Kezdőérték beállítása az első elemmel
        T legjobbMegoldas = generatorFunc(1);
        float maxErtek = fitnessFunc(legjobbMegoldas);

        // Mivel az elsőt már megnéztük, nullázzuk a számlálót, vagy 1-ről indítjuk a ciklust
        // Az eredeti logika szerint 1..m-ig megy a generálás.
        // Itt most végigmegyünk a maradékon.

        for (int i = 2; i <= limit; i++)
        {
            T aktualisJelolt = generatorFunc(i);
            float aktualisErtek = fitnessFunc(aktualisJelolt);

            LepesSzam++; // Lépésszám növelése

            // Ha jobbat találtunk, csere
            if (aktualisErtek > maxErtek)
            {
                maxErtek = aktualisErtek;
                legjobbMegoldas = aktualisJelolt;
            }
        }

        return legjobbMegoldas;
    }
}

public class NyersEroHatizsakPakolas
{
    public int LepesSzam { get; private set; }

    private readonly HatizsakProblema context; // 'problema' átnevezve
    private bool[] cachedResult; // 'legjobbPakolas' átnevezve

    public NyersEroHatizsakPakolas(HatizsakProblema problema)
    {
        this.LepesSzam = 0;
        this.context = problema;
    }

    public bool[] Generator(int index)
    {
        int elementCount = context.n;
        if (elementCount == 0) return new bool[0];

        bool[] selection = new bool[elementCount];

        for (int bit = 0; bit < elementCount; bit++)
        {
            // MÁSIK LOGIKA:
            // Az eredeti: (i & (1 << j)) != 0
            // Ez: jobbra toljuk a biteket és megnézzük az utolsót. Matematikailag ugyanaz, de a kód más.
            selection[bit] = ((index >> bit) & 1) == 1;
        }
        return selection;
    }

    public float Josag(bool[] pakolas)
    {
        // Tömörített logika: ternáris operátor használata if-else helyett
        return context.Ervenyes(pakolas) ? context.OsszErtek(pakolas) : -1f;
    }

    public bool[] OptimalisMegoldas()
    {
        if (context.n == 0)
        {
            cachedResult = new bool[0];
            LepesSzam = 0;
            return cachedResult;
        }

        // Bitshift használata a hatványozás helyett (1 << n ugyanaz mint 2^n)
        int range = 1 << context.n;

        // Generic példányosítása
        var solver = new NyersEro<bool[]>(range, Generator, Josag);

        cachedResult = solver.OptimalisMegoldas();

        // Frissítjük a lépésszámot
        LepesSzam = solver.LepesSzam;

        return cachedResult;
    }

    public float OptimalisErtek()
    {
        // Null coalescing ellenőrzés
        if (cachedResult == null)
        {
            OptimalisMegoldas();
        }
        return context.OsszErtek(cachedResult);
    }
}