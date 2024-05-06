using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace Editor_de_Grafos
{
    public class Grafo : GrafoBase, iGrafo
    {
        Random randNum = new Random();
        private bool[] visitado;

        private Color corAleatoria(List<Color> coresUsadas)
        {   // lista de cores com 26 cores contrastantes
            List<Color> cores = new List<Color> 
            {   Color.Silver, Color.DimGray, Color.SlateGray, Color.DarkSlateGray, 
                Color.Salmon, Color.Crimson, Color.DarkRed, Color.BurlyWood, 
                Color.SaddleBrown, Color.DarkKhaki, Color.Gold, Color.DeepPink, 
                Color.OrangeRed, Color.YellowGreen, Color.GreenYellow, Color.Lime, 
                Color.MediumSpringGreen, Color.SeaGreen, Color.DarkGreen, Color.Olive, 
                Color.Teal, Color.Cyan, Color.DodgerBlue, Color.BlueViolet, Color.DarkMagenta, Color.Indigo 
            };

            //remove as cores usadas da lista
            foreach (Color cor in coresUsadas)
                cores.Remove(cor);

            if (cores.Count != 0)
                return cores[randNum.Next(0, cores.Count - 1)]; //retorna uma cor aleatoria da lista que nao foi usada
            else
                return Color.FromArgb(randNum.Next(256), randNum.Next(256), randNum.Next(256)); //se todas as cores foram utilizadas, retorna uma cor criada randomicamente
        }

        public void desmarcarGrafo()
        {
            List<Vertice> vertice = vertices.FindAll(x => x.getCor() != Color.Blue);
            List<Aresta> aresta = arestas.FindAll(x => x.getCor() != Color.Blue);

            foreach (Vertice v in vertice)
                v.setCor(Color.Blue);

            foreach (Aresta a in aresta)
                a.setCor(Color.Blue);
        }

        public void completarGrafo()
        {
            for (int i = 0; i < getN(); i++)
            {
                if (this.Controls.Contains(getVertice(i))) //excluindo as vertices deletadas
                {
                    for (int j = 0; j < getN(); j++)
                    {
                        if (getAresta(i, j) == null && i != j && this.Controls.Contains(getVertice(j))) //excluindo as vertices deletadas
                        {
                            if (getPesosAleatorios())
                                setAresta(i, j, randNum.Next(1, 100));
                            else
                                setAresta(i, j, 1);
                        }
                    }
                }
            }
        }

        public String paresOrdenados()
        {
            string str = "E = {";
            for (int i = 0; i < getN(); i++)
            {
                for (int j = 0; j < getN(); j++)
                {
                    if (getAresta(i, j) != null)
                    {
                        str += " (" + getVertice(i).getRotulo() + ", " + getVertice(j).getRotulo() + ")";
                    }
                }
            }
            str += " }";
            return str;
        }

        public bool isEuleriano()
        {
            for (int i = 0; i < getN(); i++)
            {
                if (grau(i) % 2 != 0)
                    return false;
            }
            return (getN() > 0);
        }

        public bool isUnicursal()
        {
            int impar = 0;
            for (int i = 0; i < getN(); i++)
            {
                if (grau(i) % 2 != 0)
                    impar++;
            }
            return impar == 2;
        }

        public void preProfundidade(int v)
        {
            desmarcarGrafo();
            visitado = new bool[getN()];
            visitado[v] = true;
            getVertice(v).setCor(Color.Red);
            for (int i = 0; i < getN(); i++)
            {
                if (getAresta(v, i) != null && !visitado[i])
                {
                    getAresta(v, i).setCor(Color.Red);
                    profundidade(i);
                }
            }
        }

        public void profundidade(int v)
        {
            visitado[v] = true;
            getVertice(v).setCor(Color.Red);
            for (int i = 0; i < getN(); i++)
            {
                if (getAresta(v, i) != null && !visitado[i])
                {
                    getAresta(v, i).setCor(Color.Red);
                    profundidade(i);
                }
            }
        }

        public void largura(int v)
        {
            desmarcarGrafo();
            visitado = new bool[getN()];
            Fila f = new Fila(getN());
            f.enfileirar(v);
            visitado[v] = true;
            getVertice(v).setCor(Color.Red);
            while (!f.vazia())
            {
                v = f.desenfileirar();
                for (int i = 0; i < getN(); i++)
                {
                    if (getAresta(v, i) != null && !visitado[i])
                    {
                        visitado[i] = true;
                        f.enfileirar(i);
                        getVertice(i).setCor(Color.Red);
                        getAresta(v, i).setCor(Color.Red);
                    }
                }
            }
        }

        public bool isArvore()
        {
            preProfundidade(0);
            for (int i = 0; i < getN(); i++)
            {
                for (int j = 0; j < getN(); j++)
                {
                    if (getAresta(i, j) != null && getAresta(i, j).getCor() != Color.Red)
                        return false;
                }
            }

            return true;
        }

        public void AGM()
        {
            int custoTotal = 0;
            int auxV = 0;
            Aresta menorAresta = null;
            List<int> visitados = new List<int>();

            desmarcarGrafo();
            getVertice(0).setCor(Color.Red);
            visitados.Add(0);

            while (visitados.Count < getControls()) //getControls pega todas vertices que nao foram excluidas
            {
                foreach (int vi in visitados)
                {
                    foreach (Vertice vertice in getAdjacentes(vi))
                    {
                        if (!visitados.Contains(vertice.getNum()))
                        {
                            if (menorAresta == null || getAresta(vi, vertice.getNum()).getPeso() < menorAresta.getPeso())
                            {
                                menorAresta = getAresta(vi, vertice.getNum());
                                auxV = vertice.getNum();
                            }
                        }
                    }
                }

                custoTotal += menorAresta.getPeso();
                menorAresta.setCor(Color.Red);
                getVertice(auxV).setCor(Color.Red);
                visitados.Add(auxV);
                menorAresta = null;
            }

            MessageBox.Show("Custo total: " + custoTotal);
        }

        public void caminhoMinimo(int origem, int destino)
        {// algoritmo de Dijkstra
            Vertice[] antecessor = new Vertice[getN()];
            int[] estimativa = new int[getN()];
            bool[] fechado = new bool[getN()];

            for (int i = 0; i < estimativa.Length; i++)
                estimativa[i] = int.MaxValue;

            antecessor[origem] = getVertice(origem);
            estimativa[origem] = 0;
            fechado[origem] = true;
            int v = origem;

            while (!fechado[destino])
            {
                foreach (Vertice vertice in getAdjacentes(v))
                {
                    if (!fechado[vertice.getNum()])
                    {
                        int novaEstimativa = estimativa[v] + getAresta(v, vertice.getNum()).getPeso();
                        if (novaEstimativa < estimativa[vertice.getNum()])
                        {
                            estimativa[vertice.getNum()] = novaEstimativa;
                            antecessor[vertice.getNum()] = getVertice(v);
                        }
                    }
                }

                int menorEstimativa = int.MaxValue;
                for (int k = 0; k < getN(); k++)
                {
                    if (!fechado[k] && estimativa[k] < menorEstimativa)
                    {
                        menorEstimativa = estimativa[k];
                        v = k;
                    }
                }

                fechado[v] = true;
            }
            //colorir grafo
            desmarcarGrafo();

            v = destino;
            while (v != origem)
            {
                getAresta(v, antecessor[v].getNum()).setCor(Color.Red);
                v = antecessor[v].getNum();
            }

            getVertice(origem).setCor(Color.Green);
            getVertice(destino).setCor(Color.Yellow);

            MessageBox.Show("Custo mínimo: " + estimativa[destino]);
        }

        public int numeroCromatico()
        {
            int v = 0;

            setVerticeMarcado(null);
            desmarcarGrafo();

            List<Color> cores = new List<Color>();
            Color ca = corAleatoria(cores);
            visitado = new bool[getN()];
            Fila f = new Fila(getN());

            f.enfileirar(v);
            visitado[v] = true;
            getVertice(v).setCor(ca);
            cores.Add(ca);

            while (!f.vazia())
            {
                v = f.desenfileirar();
                for (int i = 0; i < getN(); i++)
                {
                    if (getAresta(v, i) != null && !visitado[i])
                    {
                        List<Color> coresAux = new List<Color>();
                        coresAux.AddRange(cores.ToArray());
                        visitado[i] = true;
                        f.enfileirar(i);
                        getAresta(v, i).setCor(Color.Red);

                        foreach (Vertice vertice in getAdjacentes(i))
                            coresAux.RemoveAll(x => x == vertice.getCor());

                        if (coresAux.Count != 0)
                            getVertice(i).setCor(coresAux[0]);
                        else
                        {
                            ca = corAleatoria(cores);
                            cores.Add(ca);
                            getVertice(i).setCor(ca);
                        }

                    }
                }
            }
            return cores.Count;
        }

        public int indiceCromatico()
        {
            if (arestas.Count > 0)
            {
                setVerticeMarcado(null);
                desmarcarGrafo();

                List<Color> cores = new List<Color>();
                Color ca;
                List<Aresta> arestaVisitada = new List<Aresta>();

                for (int i = 0; i < getN(); i++)
                {
                    for (int j = 0; j < getN(); j++)
                    {
                        Aresta a = getAresta(i, j);
                        if (a != null && !arestaVisitada.Contains(getAresta(i, j)))
                        {
                            List<Color> coresAux = new List<Color>();
                            coresAux.AddRange(cores.ToArray());
                            arestaVisitada.Add(a);

                            foreach (Vertice v in getAdjacentes(i))
                                coresAux.RemoveAll(x => x == getAresta(i, v.getNum()).getCor());

                            foreach (Vertice v in getAdjacentes(j))
                                coresAux.RemoveAll(x => x == getAresta(j, v.getNum()).getCor());

                            if (coresAux.Count != 0)
                                a.setCor(coresAux[0]);
                            else
                            {
                                ca = corAleatoria(cores);
                                cores.Add(ca);
                                a.setCor(ca);
                            }
                        }
                    }
                }

                return cores.Count;
            }
            else
                return 0;
        }
    }
}