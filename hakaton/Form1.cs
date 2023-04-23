namespace hakaton
{
    public partial class Form1 : Form
    {
        private int loopCounter = 0;
        public class Beam
        {

            public Beam(double beamAngleRad, double beamDistM, PointF beamPoint)
            {
                BeamAngleRad = beamAngleRad;
                BeamDistM = beamDistM;
                BeamPoint = beamPoint;
            }
            public Beam()
            {

            }
            public double BeamAngleRad { get; set; }
            public double BeamDistM { get; set; }

            public PointF BeamPoint { get; set; }

        }
        public class Laser
        {
            public List<PointF> beamList = new List<PointF>();
            public Laser(string? timeInSeconds, int numberOfBeams, List<PointF> beamList)
            {
                TimeInSeconds = timeInSeconds;
                NumberOfBeams = numberOfBeams;
                BeamList = beamList;
            }

            public Laser()
            {
                BeamList = new List<PointF>();
            }

            public string? TimeInSeconds { get; set; }
            public int NumberOfBeams { get; set; }
            public List<PointF> BeamList { get; set; }
        }

        public class Field
        {
            List<PointF> coordinates = new List<PointF>();
            public Field(int polygonId, List<PointF> coordinates)
            {
                PolygonID = polygonId;
                Coordinates = coordinates;
            }
            public Field()
            {
                Coordinates = new List<PointF>();
            }
            public int PolygonID { get; set; }
            public List<PointF> Coordinates { get; set; }

        }
        public Form1()
        {
            InitializeComponent();
        }

        List<Laser> laserList = new List<Laser>();
        List<Field> fieldList = new List<Field>();
        List<Field> fieldListAdv = new List<Field>();

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Text = "Next";

            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);


            Pen penBlue = new Pen(Color.Blue);
            Pen penBlack = new Pen(Color.Black);
            Pen penRed = new Pen(Color.Red);




            penBlack.Width = 3;
            penBlue.Width = 2;
            penRed.Width = 3;


            float x0 = 500.0F, y0 = 300.0F;


            for (int i = loopCounter; i < laserList.Count; i++)
            {

                int counter = fieldList.Count;
                for (int j = 0; j < laserList[i].BeamList.Count; j++)
                {

                    int active = 0;
                    if (j > 13 && j < laserList[i].BeamList.Count - 14)
                    {
                        if (radioButton1.Checked == true)
                        {
                            for (int k = 0; k < fieldList.Count; k++)
                            {
                                if (CheckField(laserList[i].BeamList[j], fieldList[k].Coordinates) == false) active++;
                            }
                            if (active < counter) counter = active;
                        }
                        if (radioButton2.Checked == true)
                        {
                            for (int k = 0; k < fieldListAdv.Count; k++)
                            {
                                if (CheckField(laserList[i].BeamList[j], fieldListAdv[k].Coordinates) == false) active++;
                            }
                            if (active < counter) counter = active;
                        }
                    }

                    float x1 = laserList[i].BeamList[j].X;
                    float y1 = laserList[i].BeamList[j].Y;
                    float x2 = laserList[i].BeamList[(j + 1) % laserList[i].BeamList.Count].X;
                    float y2 = laserList[i].BeamList[(j + 1) % laserList[i].BeamList.Count].Y;

                    g.DrawLine(penBlue, x0 + (x1 * 50), y0 - (y1 * 50), x0 + (x2 * 50), y0 - (y2 * 50));
                    pictureBox1.Image = bmp;


                }

                if (radioButton1.Checked == true)
                {
                    for (int j = 0; j < fieldList.Count; j++)
                    {
                        for (int k = 0; k < fieldList[j].Coordinates.Count; k++)
                        {
                            float x1 = fieldList[j].Coordinates[k].X;
                            float y1 = fieldList[j].Coordinates[k].Y;
                            float x2 = fieldList[j].Coordinates[(k + 1) % fieldList.Count].X;
                            float y2 = fieldList[j].Coordinates[(k + 1) % fieldList.Count].Y;
                            g.DrawLine(penBlack, x0 + (x1 * 50), y0 - (y1 * 50), x0 + (x2 * 50), y0 - (y2 * 50));
                            pictureBox1.Image = bmp;
                        }
                    }
                }
                if (radioButton2.Checked == true)
                {
                    for (int j = 0; j < fieldListAdv.Count; j++)
                    {
                        for (int k = 0; k < fieldListAdv[j].Coordinates.Count - 1; k++)
                        {
                            float x1 = fieldListAdv[j].Coordinates[k].X;
                            float y1 = fieldListAdv[j].Coordinates[k].Y;
                            float x2 = fieldListAdv[j].Coordinates[k + 1].X;
                            float y2 = fieldListAdv[j].Coordinates[k + 1].Y;
                            g.DrawLine(penBlack, x0 + (x1 * 50), y0 - (y1 * 50), x0 + (x2 * 50), y0 - (y2 * 50));
                            pictureBox1.Image = bmp;
                        }
                    }
                }
                string type = "Basic";
                if (radioButton1.Checked == true)
                {
                    type = "Basic";
                }
                if (radioButton2.Checked == true)
                {
                    type = "Advanced";
                }
                this.dataGridView1.Rows.Add(laserList[i].TimeInSeconds, counter, type);
                //label2.Text = counter.ToString();
                loopCounter = i + 1;
                break;

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;

            using (StreamReader reader = new StreamReader("fields_basic.csv"))
            {
                string basic;
                while ((basic = reader.ReadLine())
                    != null)
                {
                    Field field = new Field();
                    string[] parts = basic.Split(',');
                    field.PolygonID = Convert.ToInt32(parts[0]);
                    for (int i = 1; i < parts.Length; i += 2)
                    {
                        float x = float.Parse(parts[i]);
                        float y = float.Parse(parts[i + 1]);
                        PointF cord = new PointF(x, y);
                        field.Coordinates.Add(cord);
                    }
                    fieldList.Add(field);
                }
            }
            using (StreamReader reader = new StreamReader("fields_advanced.csv"))
            {
                string basic;
                while ((basic = reader.ReadLine())
                    != null)
                {
                    Field field = new Field();
                    string[] parts = basic.Split(',');
                    field.PolygonID = Convert.ToInt32(parts[0]);
                    for (int i = 1; i < parts.Length; i += 2)
                    {
                        float x = float.Parse(parts[i]);
                        float y = float.Parse(parts[i + 1]);
                        PointF cord = new PointF(x, y);
                        field.Coordinates.Add(cord);
                    }
                    fieldListAdv.Add(field);
                }
            }
            using (StreamReader laserReader = new StreamReader("laser.csv"))
            {
                string line;


                while ((line = laserReader.ReadLine())
                    != null)
                {
                    Laser laser = new Laser();
                    string[] parts = line.Split(',');
                    laser.TimeInSeconds = parts[0];
                    laser.NumberOfBeams = Convert.ToInt32(parts[1]);
                    for (int i = 2; i < parts.Length; i += 2)
                    {
                        double beamAngle = float.Parse(parts[i]);
                        float beamDist = float.Parse(parts[i + 1]);

                        float xLaser, yLaser;

                        xLaser = (float)(Math.Cos(beamAngle)) * beamDist;
                        yLaser = (float)(Math.Sin(beamAngle)) * beamDist;

                        PointF point = new PointF(xLaser, yLaser);
                        laser.BeamList.Add(point);

                    }
                    laserList.Add(laser);


                }

            }
        }

        private static float CrossProduct(float x1, float y1, float x2, float y2)
        {
            return x1 * y2 - y1 * x2;
        }

        public static bool CheckField(PointF point, List<PointF> field)
        {
            int windingNumber = 0;


            for (int i = 0; i < field.Count; i++)
            {
                PointF p1 = field[i];
                PointF p2 = field[(i + 1) % field.Count];



                float crossProduct = CrossProduct(p2.X - p1.X, p2.Y - p1.Y, point.X - p1.X, point.Y - p1.Y);
                if (crossProduct == 0) continue;

                if (crossProduct > 0 && p1.Y <= point.Y && p2.Y > point.Y)
                {
                    windingNumber++;
                }
                else if (crossProduct < 0 && p2.Y <= point.Y && p1.Y > point.Y)
                {
                    windingNumber--;
                }
            }

            return windingNumber != 0;



        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            for (int i = loopCounter; i < laserList.Count; i++)
            {

                int counter = fieldList.Count;
                for (int j = 0; j < laserList[i].BeamList.Count; j++)
                {

                    int active = 0;
                    if (j > 13 && j < laserList[i].BeamList.Count - 14)
                    {
                        if (radioButton1.Checked == true)
                        {
                            for (int k = 0; k < fieldList.Count; k++)
                            {
                                if (CheckField(laserList[i].BeamList[j], fieldList[k].Coordinates) == false) active++;
                            }
                            if (active < counter) counter = active;
                        }
                        if (radioButton2.Checked == true)
                        {
                            for (int k = 0; k < fieldListAdv.Count; k++)
                            {
                                if (CheckField(laserList[i].BeamList[j], fieldListAdv[k].Coordinates) == false) active++;
                            }
                            if (active < counter) counter = active;
                        }
                    }
                    string type = "Basic";
                    if (radioButton1.Checked == true)
                    {
                        type = "Basic";
                    }
                    if (radioButton2.Checked == true)
                    {
                        type = "Advanced";
                    }
                    this.dataGridView1.Rows.Add(laserList[i].TimeInSeconds, counter, type);
                }
            }
        }
    }
}