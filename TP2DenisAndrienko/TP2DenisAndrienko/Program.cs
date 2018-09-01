using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP2DenisAndrienko
{
    class Program
    {
        static void Main(string[] args)
        {
            // TP2 : compression par redondance
            // Denis Andrienko
            // l'algorithme de compression Bzip

            int[,] matrice = new int[3, 3];
            int[] t = new int[10] { 1, 1, 3, 4,4,5,5,5,8,5};
            int[] r = new int[256];
            int[] tableauDecomp = new int[t.Length];
            int[,,] matrice3D = new int[2, 2, 3] { { { 1, 1, 1 }, { 1, 2, 1 } }, { { 2, 1, 1 }, { 2, 2, 2 } } }; ;
            
            // ---------- executer et tester-----------------------------------
                     
            //Console.Write(minChar(t));
            AfficherResultats(t);
            Console.WriteLine("\n");
            //Console.Write(tailleCompression(t));
            Console.WriteLine("\n");
            AfficherResultats(Compression(t));
            Console.WriteLine("\n");
            Decompresse(t,tableauDecomp);
            Console.WriteLine("\n");
            AfficherResultats(tableauDecomp);
            Console.WriteLine("\n");
            RemplirMatrice(matrice);
            Console.WriteLine("\n");
            AfficherResultats(Compression(ImageToTableau(matrice)));
            Console.WriteLine("\n");
            Decompresse(ImageToTableau(matrice), tableauDecomp);
            Console.WriteLine("\n");
            AfficherResultats(tableauDecomp);
            Console.WriteLine("\n");
            AfficherMatrice(Tableau2Image(tableauDecomp));
            AfficherMatrice3D(matrice3D);
            Console.WriteLine("\n");
            AfficherResultats(Image3DToTableau(matrice3D));
            Console.WriteLine("\n");
            AfficherMatrice3D(Tableau3DImage(Image3DToTableau(matrice3D)));
            Console.ReadKey();
                   

            //--------------------------------------------------------------------

            int[] ImageToTableau(int[,] image)
            {
                int k = 0;

                int[] uneDemention = new int[image.GetLength(0) * image.GetLength(1)];
                
                for (int i = 0; i < image.GetLength(0); i++)
                {
                    for (int j = 0; j < image.GetLength(1); j++)
                    {
                        uneDemention[k] = image[i, j];
                        k++;
                    }
                }
                return uneDemention;
            }
                        
            int[,] Tableau2Image(int[] tableauDecompresse)
            {
                int[,] image2D = new int[matrice.GetLength(0), matrice.GetLength(1)];
                int k = 0;
                for (int i = 0; i < image2D.GetLength(0); i++)
                {
                    for (int j = 0; j < image2D.GetLength(1); j++)
                    {
                        image2D[i, j] = tableauDecompresse[k];
                        k++;
                    }
                }
                return image2D;

            }
            
            int[] Image3DToTableau(int[,,] image)
            {
                int k = 0;
                
                int[] uneDemention = new int[image.GetLength(0) * image.GetLength(1) * image.GetLength(2)];

                for (int x = 0; x < image.GetLength(0); x++)
                {
                    for (int y = 0; y < image.GetLength(1); y++)
                    {
                        for (int z = 0; z < image.GetLength(2); z++)
                        {
                            uneDemention[k] = image[x, y, z];
                            k++;
                        }
                                                
                    }
                   
                }
                return uneDemention;
            }

            int[,,] Tableau3DImage(int[] tableauDecompresse)
            {
                int[,,] image3D = new int[matrice3D.GetLength(0), matrice3D.GetLength(1), matrice3D.GetLength(2)];
                int k = 0;

                for (int x = 0; x < matrice3D.GetLength(0); x++)
                {
                    for (int y = 0; y < matrice3D.GetLength(1); y++)
                    {
                        for (int z = 0; z < matrice3D.GetLength(2); z++)
                        {
                            matrice3D[x, y, z] = tableauDecompresse[k];
                            k++;
                        }
                       
                    }

                }
                return matrice3D;

            }

            int nb_occ(int[] tableau, int element)

            {
                int occurrence = 0;
                for (int index = 0; index < tableau.Length; index++)
                {
                    if (tableau[index] == element)
                    {
                        occurrence += 1;
                    }
                }
                return occurrence;
            }

            void occurrences(int[] arrayDeElement, int[] arrayDeIndex)
            {
                for (int i = 0; i < t.Length; i++)
                {
                    arrayDeIndex[arrayDeElement[i]]++;

                }
            }

            int minChar(int[] arrayDeElement)
            {
                occurrences(t, r);
                int i = 0;
                int resultat = 0;
                while (i < 255)
                {
                    if (r[i] == 0) { i++; }
                    else if (r[i] != 0 && r[i] == r[0]) { resultat = i; return resultat; }
                    else if (r[i] != 0 && r[i] != r[0]) { resultat -= i; return resultat; }

                }

                return resultat;

            }
                       
            // methode de calculer de nombre des elementes de tableau compressé

            int tailleCompression(int[] arrayDeElement)
            {
                int taille = 0;
                int i = 1;

                while (i < arrayDeElement.Length)
                {
                    int counter = 0; // nombre de repetition
                    while (i < arrayDeElement.Length)
                    {

                        if (arrayDeElement[i] == arrayDeElement[i - 1])
                        {
                            counter += 1;
                            i++;
                        }
                        else break;

                    }
                    
                    if (counter == 0) { taille += 1; }
                    else
                    {
                        taille += 3;
                    }
                    if (i == arrayDeElement.Length-1)
                    {
                        if (arrayDeElement[i] != arrayDeElement[i - 1]) { taille ++; } else { taille++; }
                    }

                    i++;

                }

                return taille;

            }

            //methode de compresser du tableau

            int[] Compression(int[] tableau)
            {
                int tailleDeTableau = tailleCompression(t);

                int[] tableauCompresse = new int[tailleDeTableau];


                int marqueur = minChar(t);
                int counter = 0;
                int j = 0;


                for (int index = 1; index < tableau.Length; index++)
                {
                    while (tableau[index] == tableau[index - 1] && index < tableau.Length - 1)
                    {
                        counter++;
                        index++;

                    }
                    counter++;
                    if (counter > 1)
                    {
                        tableauCompresse[j] = marqueur;
                        tableauCompresse[j + 1] = counter - 1;
                        tableauCompresse[j + 2] = tableau[index - 1];
                        j += 3;
                        counter = 0;
                    }
                    else
                    {
                        
                        tableauCompresse[j] = tableau[index - 1];
                        j += 1;
                        counter = 0;
                    }
                    if (index == tableau.Length - 1)
                    {
                        if (tableau[index] != tableau[index - 1])
                        {
                            tableauCompresse[tableauCompresse.Length - 1] = tableau[index];

                        }
                        else
                        {
                            tableauCompresse[tableauCompresse.Length - 3] = marqueur;
                            tableauCompresse[tableauCompresse.Length - 2] = counter - 1;
                            tableauCompresse[tableauCompresse.Length - 1] = tableau[index];
                        }
                    }
                   

                }

                return tableauCompresse;
            }

            // methode de decompresser un tableau compressé

            void Decompresse(int[] tableauCompresse, int[] tableauDecompresse)
            {
                int marquer = minChar(tableauCompresse);
                int indexJ = 0;
                for (int index = 0; index < tableauCompresse.Length; index++)
                {

                    if (tableauCompresse[index] == marquer)
                    {
                        for (int k = 1; k <= tableauCompresse[index + 1]+1; k++)
                        {
                           
                            tableauDecompresse[indexJ] = tableauCompresse[index + 2];
                            indexJ++;
                        }
                        index += 2;
                    }
                    else
                    {
                        tableauDecompresse[indexJ] = tableauCompresse[index];
                        
                        indexJ++;
                    }
                    
                }

            }

            //methode d'afficher les resultats

            void AfficherResultats(int[] resultat)
            {
                for (int i = 0; i < resultat.Length; i++) { Console.Write($" {resultat[i]} "); }
            }

        }
        // methode pour remplir une matrice
        static void RemplirMatrice(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.WriteLine($"Remplir le element de matrice {i},{j}");

                    matrix[i, j] = int.Parse(Console.ReadLine());
                }
            }

        }
        // methode d'afficher une matrice
        static void AfficherMatrice(int[,] resultat)
        {
            for (int i = 0; i < resultat.GetLength(0); i++)
            {
                for (int j = 0; j < resultat.GetLength(1); j++)
                {
                    Console.Write($"\t{resultat[i,j]}"); 

                }
                Console.Write("\n");
            }
        }

        // methode d'afficher une matrice3D
        static void AfficherMatrice3D(int[,,] resultat)
        {
            for (int i = 0; i < resultat.GetLength(0); i++)
            {
                for (int j = 0; j < resultat.GetLength(1); j++)
                {
                    for (int k = 0; k < resultat.GetLength(2); k++)
                    {
                        Console.Write($"\t{resultat[i, j,k]}");
                    }
                    Console.Write("\n");

                }
                Console.Write("\n");
            }
        }
    }
}

