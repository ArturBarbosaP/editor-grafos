using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using Microsoft.VisualBasic;

namespace Editor_de_Grafos
{
    public class GrafoBase : Panel
    {
        public static int n; // número de vértices
        public static List<Vertice> vertices;
        public static List<Aresta> arestas;
        public static Aresta[,] matAdj; // matriz de adjacências
        public static Vertice vMarcado;
        public static bool exibirPesos, pesosAleatorios, inserirPesos, excluirVertice, excluirAresta;
        Random randNum = new Random();

        public GrafoBase()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            n = 0;
            vertices = new List<Vertice>();
            arestas = new List<Aresta>();
            vMarcado = null;
            matAdj = new Aresta[500, 500];
            exibirPesos = true;
            pesosAleatorios = false;
        }

        #region Get e Set

        public bool getExibirPesos()
        {
            return exibirPesos;
        }

        public bool getPesosAleatorios()
        {
            return pesosAleatorios;
        }

        public bool getInserirPesos()
        {
            return inserirPesos;
        }

        public bool getExcluirVertice()
        {
            return excluirVertice;
        }

        public bool getExcluirAresta()
        {
            return excluirAresta;
        }

        public Vertice getVertice(int v)
        {
            int i;
            Vertice aux;
            for (i = 0; i < vertices.Count; i++)
            {
                aux = vertices[v];
                if (aux.getNum() == v)
                    return aux;
            }
            return null;
            //return vertices.Find(x => x.getNum() == v);
            //return vertices[v];
        }

        public Vertice getVertice(string v)
        {
            return vertices.Find(x => x.getRotulo() == v);
        }

        public Vertice getVerticeMarcado()
        {
            return vMarcado;
        }

        public Aresta getAresta(int i, int j)
        {
            return matAdj[i, j];
        }

        public int getN()
        {
            return n;
        }

        public int getControls()
        {
            return this.Controls.Count;
        }

        public List<Vertice> getAdjacentes(int v)
        {
            List<Vertice> lista = new List<Vertice>();
            for (int i = 0; i < n; i++)
                if (matAdj[v, i] != null)
                    lista.Add(getVertice(i));
            return lista;
        }

        public void setExibirPesos(bool e)
        {
            exibirPesos = e;
        }

        public void setPesosAleatorios(bool e)
        {
            pesosAleatorios = e;
        }

        public void setInserirPesos(bool e)
        {
            inserirPesos = e;
        }

        public void setExcluirVertice(bool e)
        {
            excluirVertice = e;
        }

        public void setExcluirAresta(bool e)
        {
            excluirAresta = e;
        }

        public void setVerticeMarcado(Vertice v)
        {
            if (vMarcado != null)
                vMarcado.desmarcar();
            vMarcado = v;
            Refresh();
        }

        public void setAresta(int i, int j, int peso)
        {
            Aresta a = new Aresta(peso, Color.Blue, this);
            arestas.Add(a);
            matAdj[i, j] = matAdj[j, i] = a;
            Refresh();
        }

        public void deleteAresta(int i, int j)
        {
            arestas.Remove(getAresta(i, j));
            matAdj[i, j] = matAdj[j, i] = null;
            Refresh();
        }

        #endregion ---------------------------------------------------------------------------------------------------

        #region Funçoes

        public void limpar()
        {
            n = 0;
            vertices.Clear(); // limpa a lista de vértices
            arestas.Clear();
            vMarcado = null; // limpa a referencia a qualquer vertice marcado
            matAdj = new Aresta[500, 500];

            this.Controls.Clear();
            this.Refresh(); // redesenha a tela
        }

        public void DeleteVertice()
        {
            Vertice v = getVerticeMarcado();
            foreach (Vertice vertice in getAdjacentes(v.getNum()))
            {
                deleteAresta(v.getNum(), vertice.getNum());
            }
            setVerticeMarcado(null);
            //vertices.Remove(v);
            this.Controls.Remove(v);
        }

        public void AddVertice(int num, string rotulo, int x, int y)
        {
            Vertice v = new Vertice(num, rotulo, x, y, this);
            vertices.Add(v);
            this.Controls.Add(v);
        }

        public void abrirArquivo(string path)
        {
            try
            {

                int i, j;
                string[] p;
                int ultimo, vx, vy;
                string nome;
                StreamReader s = new StreamReader(path);

                limpar();

                n = int.Parse(s.ReadLine());
                ultimo = int.Parse(s.ReadLine());

                s.ReadLine();
                for (i = 0; i < n; i++)
                {
                    string[] data = s.ReadLine().Split(' ');
                    vx = int.Parse(data[0]); // coordenada x
                    vy = int.Parse(data[1]); // coordenada y
                    nome = data[2];
                    AddVertice(i, nome, vx, vy);
                }
                s.ReadLine();
                s.ReadLine();
                for (i = 0; i < n; i++)
                {
                    // for(j = 0; j < i; j++)
                    // {
                    //s.nextInt(); 
                    s.ReadLine();

                    // }
                }

                for (i = 1; i < n; i++)
                {
                    p = s.ReadLine().TrimEnd().Split(' ');
                    for (j = 0; j < i; j++)
                    {

                        if (int.Parse(p[j]) > 0) {
                            setAresta(i, j, int.Parse(p[j]));
                        }
                    }
                }
                s.Close();
                this.Refresh();
            }
            catch (Exception eX)
            {
                MessageBox.Show("Erro ao abrir arquivo. " + eX.Message);
            }
        }

        public void SalvarArquivo(string path)
        {
            try
            {
                int i, j;
                string temp = "";
                StreamWriter r = new StreamWriter(path);

                r.WriteLine(n);
                r.WriteLine(n);

                r.WriteLine();


                for (i = 0; i < n; i++)
                {
                    r.WriteLine(getVertice(i).getX() + " " +
                        getVertice(i).getY() + " " +
                        getVertice(i).Text);
                }

                r.WriteLine();

                for (i = 0; i < n; i++)
                {
                    for (j = 0; j < i; j++)
                    {
                        if (matAdj[i, j] != null)
                            temp += "1 ";
                        else
                            temp += "0 ";
                    }
                    r.WriteLine(temp);
                    temp = "";
                }

                temp = "";

                for (i = 0; i < n; i++)
                {
                    for (j = 0; j < i; j++)
                    {
                        if (matAdj[i, j] != null)
                            temp += matAdj[i, j].getPeso() + " ";
                        else
                            temp += "0 ";
                    }
                    r.WriteLine(temp);
                    temp = "";
                }
                r.Close();
            }
            catch (IOException eX)
            {
                MessageBox.Show("Erro ao gravar o arquivo. " + eX.Message);
            }
        }

        public int grau(int v)
        {
            int cont = 0;
            for (int i = 0; i < n; i++)
                if (matAdj[v, i] != null)
                    cont++;
            return cont;
        }

        public void clicouVertice(Vertice v)
        {
            if (v.getMarcado())
            {
                v.desmarcar();
                vMarcado = null;
            }
            else
            {
                v.marcar();
                if (vMarcado != null)
                {
                    if (getExcluirAresta())
                    {
                        deleteAresta(vMarcado.getNum(), v.getNum());
                        vMarcado.desmarcar();
                        vMarcado = v;
                    }
                    else
                    {
                        int peso = 1;

                        if (getInserirPesos())
                        {
                            string txt = Interaction.InputBox("Digite o peso para a aresta", "Editar peso da aresta");
                            try
                            {
                                if (txt != "")
                                    peso = int.Parse(txt);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Insira apenas números!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else if (getPesosAleatorios())
                            peso = (int)(randNum.Next(1, 100));

                        if (matAdj[vMarcado.getNum(), v.getNum()] == null)
                        {
                            setAresta(vMarcado.getNum(), v.getNum(), peso);
                            vMarcado.desmarcar();
                            vMarcado = v;

                        }
                        else
                        {
                            getAresta(vMarcado.getNum(), v.getNum()).setPeso(peso);
                            v.desmarcar();
                        }
                    }
                }
                else
                {
                    vMarcado = v;
                }
            }
            Refresh();
        }

        #endregion ----------------------------------------------------------------------------------------------------

        int i, j, tam;
        Vertice vi, vj;
        Aresta a;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            for (i = 0; i < n; i++)
            {
                for (j = 0; j < i; j++)
                {
                    if (matAdj[i, j] != null)
                    {
                        a = matAdj[i, j];
                        vi = getVertice(i);
                        vj = getVertice(j);
                        e.Graphics.DrawLine(new Pen(a.getCor()), vi.getX() + 5, vi.getY() + 5, vj.getX() + 5, vj.getY() + 5);
                        if (exibirPesos)
                        {
                            tam = (int)e.Graphics.MeasureString(a.getPeso().ToString(), new Font(Font.Name, 10, FontStyle.Regular)).Width;
                            int tam1 = (tam / 2);
                            e.Graphics.FillRectangle(new SolidBrush(Color.Gray), (vi.getX() + vj.getX() + 10) / 2 - tam1, (vi.getY() + vj.getY()) / 2 - 3.5f, tam, 18);
                            e.Graphics.DrawString(a.getPeso().ToString(), new Font(Font.Name, 10, FontStyle.Regular), new SolidBrush(Color.White), (vi.getX() + vj.getX() + 10) / 2 - tam1, (vi.getY() + vj.getY() - 5) / 2);
                        }
                    }
                }
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button == MouseButtons.Left)
                AddVertice((n++), "V" + n, e.X, e.Y);
        }
    }
}
