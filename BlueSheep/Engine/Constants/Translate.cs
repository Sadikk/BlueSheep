using BlueSheep.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BlueSheep.Engine.Constants
{
    public class Translate
    {
        #region Fields
        private static Dictionary<string, string> Dic = new Dictionary<string, string>();
        private static int ENInitialized = 0;
        private static int PTInitialized = 0;
        private static int ESInitialized = 0;
        #endregion

        #region Public Methods
        public static void TranslateUC(UserControl uc)
        {
            foreach (Control c in uc.Controls)
            {
                c.Text = GetTranslation(c.Text, MainForm.ActualMainForm.Lang);
            }
        }

        public static string GetTranslation(string content, string langue)
        {
            switch (langue)
            {
                case "FR":
                    return content;
                case "EN":
                    if (ENInitialized == 0)
                        InitEN();
                    break;
                case "PT":
                    if (PTInitialized == 0)
                        InitPT();
                    break;
                case "ES":
                    if (ESInitialized == 0)
                        InitES();
                    break;
            }
            foreach (KeyValuePair<string, string> pair in Dic)
            {
                if (content.Contains(pair.Key))
                    content = content.Replace(pair.Key, pair.Value);
            }
            return content;
        }
        #endregion

        #region Private Methods
        private static void InitEN()
        {
            Dic.Clear();
            Dic.Add("Connecté", "Connected");
            Dic.Add("Déconnecté", "Disconnected");
            Dic.Add("Dialogue", "Speaking");
            Dic.Add("Régénération", "Regenerating");
            Dic.Add("Récolte", "Gathering");
            Dic.Add("Déplacement", "Moving");
            Dic.Add("Combat", "Fighting");
            Dic.Add("Téléportation", "Teleportating");
            Dic.Add("est mort", "is dead");
            Dic.Add("Fin du tour", "Turn ended");
            Dic.Add("Equipement rapide numero", "Stuff preset number");
            Dic.Add("Trajet arrêté", "Path Stopped");
            Dic.Add("Reconnexion automatique dans minutes", "Reconnection in X minutes");
            Dic.Add("Prochain repas dans", "Next meal in");
            Dic.Add("Aucune nourriture disponible, pas de prochaine connexion", "No food available, stay disconnected.");
            Dic.Add("Connexion", "Connecting");
            Dic.Add("Je suis le chef de groupe biatch", "I'm the group leader biatch !");
            Dic.Add("Trajet chargé", "Path loaded ");
            Dic.Add("Erreur lors de la sélection du personnage", "Error during the selection of the character");
            Dic.Add("Serveur complet", "The server is full, try again later");
            Dic.Add("Echec de connexion : Dofus a été mis à jour", "Connection failure, Dofus has been updated. Try again later");
            Dic.Add("Echec de connexion : Vous êtes bannis", "Connection failure, your account has been banned");
            Dic.Add("Identification en cours", "Identification in process");
            Dic.Add("Identification réussie", "Identification successful");
            Dic.Add("Echec de l'identification", "Identification failure");
            Dic.Add("J'ai rejoint le groupe", "I joined the group");
            Dic.Add("Ancienne cellid of", "Last cellid of");
            Dic.Add("Nouvelle cellid of", "New cellid of ");
            Dic.Add("Début du combat", "Fight started");
            Dic.Add("Combat fini !", "Fight is over !");
            Dic.Add("Echec de l'ouverture du coffre", "Failed to open the chest");
            Dic.Add("File d'attente", "Position in queue");
            Dic.Add("Déconnecté du serveur", "Disconnected from the server");
            Dic.Add("Inactivité prolongée", "Prolonged inactivity");
            Dic.Add("Connexion établie au serveur", "Connected to server");
            Dic.Add("Connexion échouée", " Connection failed");
            Dic.Add("Lancement du trajet", "Path launched");
            Dic.Add("Echec de connexion : compte banni temporairement", "Connection failure, your account is temporarily banned");
            Dic.Add("Echec de connexion : compte banni", "Connection failure, your account is banned");
            Dic.Add("Echec de connexion : serveur en maintenance", "Connection failure, server's is down for maintenance");
            Dic.Add("Echec de connexion : erreur inconnue", "Connection failure : Uknown error");
            Dic.Add("Echec de connnexion : serveur déconnecté", "connection failure, servor disconnected");
            Dic.Add("Echec de connexion : serveur en sauvegarde", "Connection failure, the server is down for backup");
            Dic.Add("Echec de connexion : serveur complet", "connection failure, servor is full");
            Dic.Add("Echec de connexion : raison inconnue", "Connection failure : Unknown cause");
            Dic.Add("Récupération d'un objet du coffre", "Taking an item from the chest");
            Dic.Add("Fermeture du coffre", "Closing the chest");
            Dic.Add("Ouverture du coffre", "Opening the chest");
            Dic.Add("Dépôt d'un objet dans le coffre", "Putting an item in the chest");
            Dic.Add("est connecté", "is connected");
            Dic.Add("Fight Turn", "Fight Turn");
            Dic.Add("Sort validé", "Spell agreed-upon");
            Dic.Add("Cible en vue à la cellule", "Traget is on the cell n°");
            Dic.Add("It's Time to capture ! POKEBALL GOOOOOOOO", "It's Time to capture ! POKEBALL GOOOOOOOO");
            Dic.Add("Lancement d'un combat contre monstres de niveau", "Starting a fight against monsters of level ");
            Dic.Add("Lancement d'un sort en", "Launching a spell at");
            Dic.Add("Placement du personnage", "Character's in-fight position");
            Dic.Add("Impossible d'enclencher le déplacement", "Unable to move");
            Dic.Add("Le bot n'a pas encore reçu les informations de la map, veuillez patienter. Si le problème persiste, rapportez le beug sur forum : http://forum.bluesheepbot.com ", "The bot didn't receive the map's informations yet, please check out later, if the poblem persists, report it on the forum : http://forum.bluesheep.com");
            Dic.Add("Up de caractéristique", "Enhance characteristics");
            Dic.Add("Envoi de la réponse", "Sending the answer");
            Dic.Add("Aucune réponse disponible dans le trajet", "No answer available in the path for this ask");
            Dic.Add("au serveur de jeu", "to the game's server");
            Dic.Add("au serveur d'identification", "to the authentication's server");
            Dic.Add("La récolte n'est pas encore implémentée, veuillez attendre la mise à jour. Tenez vous au courant sur", "Gathering isn't already implemented, please wait a release. Stay updated on");
            ENInitialized = 1;
        }

        private static void InitES()
        {
            Dic.Clear();
            Dic.Add("Connecté", "Conectado");
            Dic.Add("Déconnecté", "Desconectado");
            Dic.Add("Dialogue", "Diálogo");
            Dic.Add("Régénération", "Regeneración");
            Dic.Add("Récolte", "Cosecha");
            Dic.Add("Déplacement", "Desplazamiento");
            Dic.Add("Combat", "Combate");
            Dic.Add("Téléportation", "Teleportación");
            Dic.Add("Trajet arrêté", "Camino detuvo");
            Dic.Add("Reconnexion automatique dans minutes", "Reconexión en X minutos");
            Dic.Add("Prochain repas dans heures", "Alimentación Siguiente X horas");
            Dic.Add("Aucune nourriture disponible, pas de prochaine connexion", "No hay comida disponible, mantenerse desconectado");
            Dic.Add("Connexion", "Conectando");
            Dic.Add("Je suis le chef de groupe biatch", "Yo soy el líder del grupo, perra");
            Dic.Add("Trajet chargé", "Camino cargado");
            Dic.Add("Erreur lors de la sélection du personnage", "Error durante la selección de personaje");
            Dic.Add("Serveur complet", "Servidor está llena, inténtalo más tarde");
            Dic.Add("Echec de connexion : Dofus a été mis à jour", "Error de conexión, Dofus se ha actualizado, intentarlo más tarde");
            Dic.Add("Echec de connexion : Vous êtes bannis", "Error de conexión, tu cuenta ha sido prohibido");
            Dic.Add("Identification en cours", "Identificación en curso");
            Dic.Add("Identification réussie", "Identificado con éxito");
            Dic.Add("Echec de l'identification", "Fracaso de identificación");
            Dic.Add("Bienvenue sur DOFUS, dans le Monde des Douze !", "Bienvenido a DOFUS, el Mundo de los Doce");
            Dic.Add("Il est interdit de transmettre votre identifiant ou votre mot de passe", "Se prohíbe transmitir su nombre de usuario o contraseña.");
            Dic.Add("Votre adresse IP actuelle est", "Su dirección de IP es");
            Dic.Add("Impossible de lancer ce sort, vous avez une portée de", "Incapaz de lanzar el hechizo, tiene un rango de, y está apuntando a 2");
            Dic.Add("J'ai rejoint le groupe", "Me uní a un grupo");
            Dic.Add("Ancienn cellid of", "Última ID de celda");
            Dic.Add("Nouvelle cellid of", "Nuevo ID de celda");
            Dic.Add("Début du combat", "Lucha comenzado");
            Dic.Add("GameMap Ancienne cellid of", "Última ID de celda del Mapa de Juego");
            Dic.Add("GameMap Nouvelle cellid of", "Nuevo ID de celda del Mapa de Juego");
            Dic.Add("Send ready !", "Envía listo");
            Dic.Add("Combat fini !", "Lucha ha terminado");
            Dic.Add("Echec de l'ouverture du coffre", "Error al abrir el baul");
            Dic.Add("File d'attente", "Posición en la cola");
            Dic.Add("Déconnecté du serveur", "desconectado del servidor");
            Dic.Add("Inactivité prolongée", "inactividad prolongada");
            Dic.Add("Connexion établie au serveur", "Conectado al servidor");
            Dic.Add("Connexion échouée", "Error de conexión");
            Dic.Add("Lancement du trajet", "Lanzamiento de camino");
            Dic.Add("Echec de connexion : compte banni temporairement", "Error de conexión, Su cuenta se temporalmente prohibido");
            Dic.Add("Echec de connexion : compte banni", "Error en la conexión, su cuenta está prohibida");
            Dic.Add("Echec de connexion : serveur en maintenance", "Error de conexión, el servidor es cerrado por mantenimiento");
            Dic.Add("Echec de connexion : erreur inconnue", "Error de conexión: Error desconocido");
            Dic.Add("Echec de connnexion : serveur déconnecté", "Error de conexión, servidor está desconectado");
            Dic.Add("Echec de connexion : serveur en sauvegarde", "Error de conexión: el servidor no funciona para copia de seguridad");
            Dic.Add("Echec de connexion : serveur complet", "Error de conexión, servidor está lleno");
            Dic.Add("Echec de connexion : raison inconnue", "Error de conexión: Causa desconocida");
            Dic.Add("Récupération d'un objet du coffre", "Recogiendo un elemento del baul");
            Dic.Add("Fermeture du coffre", "Cerrando el baul");
            Dic.Add("Ouverture du coffre", "Abriendo el baul");
            Dic.Add("Dépôt d'un objet dans le coffre", "Abriendo el baul");
            Dic.Add("est connecté", "se encuentra conectado");
            Dic.Add("Fight Turn", "Su vez");
            Dic.Add("Sort validé", "Hechizo validado");
            Dic.Add("Cible en vue à la cellule", "Del objetivo es el número de células.");
            Dic.Add("It's Time to capture ! POKEBALL GOOOOOOOO", "Es hora de capturar! Pokeball GOOOOOO");
            Dic.Add("Lancement d'un combat contre monstres de niveau", "Inicio de una lucha contra monstruos de nivel");
            Dic.Add("Lancement d'un sort en", "Al lanzar un hechizo sobre");
            Dic.Add("Placement du personnage", "Carácter en posición de luchar");
            Dic.Add("Impossible d'enclencher le déplacement", "No se puede mover");
            Dic.Add("Le bot n'a pas encore reçu les informations de la map, veuillez patienter. Si le problème persiste, rapportez le beug sur forum : http://forum.bluesheepbot.com", "El Bot no ha recibido información, Por favor, vuelva más tarde, si el problema persiste, notifique foro: http://forum.bluesheep.com");
            Dic.Add("Up de caractéristique", "Mejorar características");
            ESInitialized = 1;
        }

        private static void InitPT()
        {
            Dic.Clear();
            Dic.Add("Connecté", "Conectado");
            Dic.Add("Déconnecté", "Desconectado");
            Dic.Add("Dialogue", "Diálogo");
            Dic.Add("Régénération", "Regeneração");
            Dic.Add("Récolte", "Colheita");
            Dic.Add("Déplacement", "Deslocação");
            Dic.Add("Combat", "Briga");
            Dic.Add("Téléportation", "Teletransporte");
            Dic.Add("Trajet arrêté", "Trajeto Parou");
            Dic.Add("Reconnexion automatique dans minutes", "Reconexão em X minutos");
            Dic.Add("Prochain repas dans heures", "Próxima alimentação em X horas");
            Dic.Add("Aucune nourriture disponible, pas de prochaine connexion", "Sem comida disponível, permaneça desconectado");
            Dic.Add("Connexion", "Conectando");
            Dic.Add("Je suis le chef de groupe biatch", "Eu sou o líder do grupo, cadela");
            Dic.Add("Trajet chargé", "Caminho carregado");
            Dic.Add("Erreur lors de la sélection du personnage", "Erro durante seleção do personagem");
            Dic.Add("Serveur complet", "Servidor está cheio, tente novamente mais tarde");
            Dic.Add("Echec de connexion : Dofus a été mis à jour", "Erro de conexão, Dofus foi atualizado, tente novamente mais tarde");
            Dic.Add("Echec de connexion : Vous êtes bannis", "Erro de conexão, sua conta foi banida");
            Dic.Add("Identification en cours", "Identificação em processo");
            Dic.Add("Identification réussie", "Identificado com sucesso");
            Dic.Add("Echec de l'identification", "Falha de identificação");
            Dic.Add("Bienvenue sur DOFUS, dans le Monde des Douze !", "Bem vindo ao Dofus, ");
            Dic.Add("Il est interdit de transmettre votre identifiant ou votre mot de passe", "É proibido transferir);transmitir seu nome de usuário e senha");
            Dic.Add("Votre adresse IP actuelle est", "Seu endereço IP é");
            Dic.Add("Impossible de lancer ce sort, vous avez une portée de 0 à 1, et vous visez à 2 !", "");
            Dic.Add("J'ai rejoint le groupe", "Impossível lançar o feitiço, você tem um alcance de 0 a 1, e está mirando a 2");
            Dic.Add("Ancienn cellid of", "Eu ingressei em um grupo");
            Dic.Add("Nouvelle cellid of", "ùltima id de célula de");
            Dic.Add("Début du combat", "Combate começou");
            Dic.Add("GameMap Ancienne cellid of", "Última id de célula do Mapa do Jogo");
            Dic.Add("GameMap Nouvelle cellid of", "Nova id de célula do Mapa do Jogo");
            Dic.Add("Send ready !", "Enviar pronto");
            Dic.Add("Combat fini !", "Fim de combate ");
            Dic.Add("Echec de l'ouverture du coffre", "Falhou ao abrir o baú");
            Dic.Add("File d'attente", "Posição na fila");
            Dic.Add("Déconnecté du serveur", "Disconectado do servidor");
            Dic.Add("Inactivité prolongée", "Inatividade prolongada");
            Dic.Add("Connexion établie au serveur", "Conectado ao servidor");
            Dic.Add("Connexion échouée", "Conexão falhou");
            Dic.Add("Lancement du trajet", "Lançamento do caminho");
            Dic.Add("Echec de connexion : compte banni temporairement", "Falha de conexão, sua conta está temporariamente banida");
            Dic.Add("Echec de connexion : compte banni", "Falha de conexão, sua conta está banida");
            Dic.Add("Echec de connexion : serveur en maintenance", "Falha de conexão, servidor está inativo para manutenção");
            Dic.Add("Echec de connexion : erreur inconnue", "Falha de conexão: Erro desconhecido");
            Dic.Add("Echec de connnexion : serveur déconnecté", "Falha de conexão, servidor está disconectado");
            Dic.Add("Echec de connexion : serveur en sauvegarde", "Falha de conexão: o servidor está inativo para cópia de segurança");
            Dic.Add("Echec de connexion : serveur complet", "Falha de conexão, servidor está cheio");
            Dic.Add("Echec de connexion : raison inconnue", "Falha de conexão: Causa desconhecida");
            Dic.Add("Récupération d'un objet du coffre", "Pegando um item do baú");
            Dic.Add("Fermeture du coffre", "Fechando o baú");
            Dic.Add("Ouverture du coffre", "Abrindo o baú");
            Dic.Add("Dépôt d'un objet dans le coffre", "Colocando um item no baú");
            Dic.Add("est connecté", "está conectado");
            Dic.Add("Fight Turn", "Turno da luta");
            Dic.Add("Sort validé", "Feitiço validado");
            Dic.Add("Cible en vue à la cellule", "O alvo está na célula número...");
            Dic.Add("It's Time to capture ! POKEBALL GOOOOOOOO", "É hora de capturar! Pokebola vaaaaaaaaiii");
            Dic.Add("Lancement d'un combat contre monstres de niveau", "Iniciando a luta contra monstros de nível");
            Dic.Add("Lancement d'un sort en", "Lançando um feitiço em");
            Dic.Add("Placement du personnage", "Personagem em posição de luta");
            Dic.Add("Impossible d'enclencher le déplacement", "Incapaz de se mover");
            Dic.Add("Le bot n'a pas encore reçu les informations de la map, veuillez patienter. Si le problème persiste, rapportez le beug sur forum : http://forum.bluesheepbot.com ", "O Bot não recebeu informações ainda, por favor, cheque novamente mais tarde, se o problema persistir, avise no fórum: http://forum.bluesheep.com");
            Dic.Add("Up de caractéristique", "Melhorar Características");
            PTInitialized = 1;
        }
#endregion
    }
}
