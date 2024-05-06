using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Editor_de_Grafos
{
    public partial class Editor : Form
    {
        public Editor()
        {
            InitializeComponent();            
        }

        #region Botoes de Algoritmo do Menu
        private void BtParesOrd_Click(object sender, EventArgs e)
        {
            if (g.getN() != 0)
            {
                string pares = g.paresOrdenados();
                MessageBox.Show(pares, "Pares Ordenados", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Erro: não há vértices no grafo!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void BtGrafoEuleriano_Click(object sender, EventArgs e)
        {
            if (g.getN() != 0)
            {
                if (g.isEuleriano())
                    MessageBox.Show("O grafo e Euleriano!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("O grafo não e Euleriano!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Erro: não há vértices no grafo!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void BtGrafoUnicursal_Click(object sender, EventArgs e)
        {
            if (g.getN() != 0)
            {
                if (g.isUnicursal())
                    MessageBox.Show("O grafo e Unicursal!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("O grafo não e Unicursal!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Erro: não há vértices no grafo!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void profundidadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Vertice v = g.getVerticeMarcado();
            if (v != null)
                g.preProfundidade(v.getNum());
            else
                MessageBox.Show("Selecione um vertice!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void larguraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Vertice v = g.getVerticeMarcado();
            if (v != null)
                g.largura(v.getNum());
            else
                MessageBox.Show("Selecione um vertice!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void árvoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (g.getN() != 0)
            {
                if (g.isArvore())
                    MessageBox.Show("O grafo é uma árvore!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("O grafo não é uma árvore!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Erro: não há vértices no grafo!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void árvoreGeradoraMínimaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (g.getN() != 0)
                g.AGM();
            else
                MessageBox.Show("Erro: não há vértices no grafo!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void caminhoDeCustoMínimoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (g.getN() != 0)
            {
                g.setVerticeMarcado(null);
                string txt = Interaction.InputBox("Digite o rótulo da vértice de origem", "Caminho de custo mínimo");
                Vertice v1 = g.getVertice(txt);
                if (v1 != null)
                {
                    txt = Interaction.InputBox("Digite o rótulo da vértice de destino", "Caminho de custo mínimo");
                    Vertice v2 = g.getVertice(txt);
                    if (v2 != null)
                        g.caminhoMinimo(v1.getNum(), v2.getNum());
                    else
                        MessageBox.Show("Erro: não há vértices no grafo com esse rótulo!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("Erro: não há vértices no grafo com esse rótulo!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
                MessageBox.Show("Erro: não há vértices no grafo!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void númeroCromáticoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (g.getN() != 0)
            {
                int numCrom = g.numeroCromatico();
                MessageBox.Show("X(G) = " + numCrom.ToString(), "Número Cromático", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Erro: não há vértices no grafo!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void índiceCromáticoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int indCrom = g.indiceCromatico();
            if (indCrom == 0)
                MessageBox.Show("Erro: não há arestas no grafo!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show("X’(G) = " + indCrom.ToString(), "Índice Cromático", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion --------------------------------------------------------------------------------------------------

        #region Botoes de Ferramentas do Menu

        private void BtNovo_Click(object sender, EventArgs e)
        {
            g.limpar();
        }

        private void BtAbrir_Click(object sender, EventArgs e)
        {
            if(OPFile.ShowDialog() == DialogResult.OK)
            {
                g.abrirArquivo(OPFile.FileName);
                g.Refresh();
            }
        }

        private void BtSalvar_Click(object sender, EventArgs e)
        {
            if(SVFile.ShowDialog() == DialogResult.OK)
            {
                g.SalvarArquivo(SVFile.FileName);
            }
        }

        private void BtSair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtPeso_Click(object sender, EventArgs e)
        {
            if (BtPeso.Checked)
            {
                BtPeso.Checked = false;
                g.setExibirPesos(false);
            }
            else
            {
                BtPeso.Checked = true;
                g.setExibirPesos(true);
            }
            g.Refresh();
        }

        private void BtPesoAleatorio_Click(object sender, EventArgs e)
        {
            if (BtPesoAleatorio.Checked)
            {
                BtPesoAleatorio.Checked = false;
                g.setPesosAleatorios(false);
            }
            else
            {
                BtPesoAleatorio.Checked = true;
                g.setPesosAleatorios(true);
                BtInserirPeso.Checked = false;
                g.setInserirPesos(false);
            }
        }

        private void BtInserirPeso_Click(object sender, EventArgs e)
        {
            if (BtInserirPeso.Checked)
            {
                BtInserirPeso.Checked = false;
                g.setInserirPesos(false);
            }
            else
            {
                BtInserirPeso.Checked = true;
                g.setInserirPesos(true);
                BtPesoAleatorio.Checked = false;
                g.setPesosAleatorios(false);
            }
        }

        private void completarGrafoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            g.completarGrafo();
            g.setVerticeMarcado(null);
        }

        private void desmarcarGrafoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            g.desmarcarGrafo();
            g.setVerticeMarcado(null);
        }

        private void excluirVérticeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (g.getVerticeMarcado() != null)
            {
                g.DeleteVertice();
                g.desmarcarGrafo();
            }
            else
                MessageBox.Show("Selecione um vertice!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void excluirArestaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (excluirArestaToolStripMenuItem.Checked)
            {
                excluirArestaToolStripMenuItem.Checked = false;
                g.setExcluirAresta(false);
            }
            else
            {
                excluirArestaToolStripMenuItem.Checked = true;
                g.setExcluirAresta(true);
            }
        }

        private void BtSobre_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Editor de Grafos - 2024/1\n\nDesenvolvido por:\nArtur Barbosa Pinto - 72300400\n\nAlgoritmos e Estruturas de Dados II\nFaculdade COTEMIG\nSomente para fins didáticos.", "Sobre o Editor de Grafos...", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion --------------------------------------------------------------------------------------------------

        private void g_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
