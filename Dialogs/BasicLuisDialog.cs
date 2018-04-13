using System;
using System.Configuration;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

using Microsoft.Bot.Builder.Dialogs; 
using Microsoft.Bot.Connector;

namespace Microsoft.Bot.Sample.LuisBot
{
    
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object>
    {
        private int salud=0;
        
        public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"], 
            ConfigurationManager.AppSettings["LuisAPIKey"], 
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"])))
        {
        }

        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Lo siento no comprendo");
            context.Wait(MessageReceived);
        }

        [LuisIntent("saludo")]
        public async Task GreetingIntent(IDialogContext context, LuisResult result)
        {
            
             if (salud==0)
            {
                salud = 1;
            await context.PostAsync("Hola soy un robot  y mi nombre es Romina, te ayudare a resolver  dudas en el uso de  e-learning");
            await context.PostAsync("Como te llamas?");
            context.Wait(NameReceivedAsync);
              }
            else
            {
                await context.PostAsync("Ya me habias saludado");
                context.Wait(MessageReceived);

            }
  
        }
                 [LuisIntent("Despedida")]
        public async Task byeIntent(IDialogContext context, LuisResult result)
        {
            salud=0;
            await context.PostAsync("Espero pases bonito dia :)");
            context.Wait(MessageReceived);
        }
 
        [LuisIntent("Nombre")]
        public async Task CancelIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Que tal {result.Query}, en puedo ayudarte? ");
            context.Wait(MessageReceived);
        }
        [LuisIntent("ProblemaCurso")]
        public async Task ProblemaCursoIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Entiendo que tu problema se debe al funcionamiento del curso ");
            await context.PostAsync("Te recomiendo que envies los detalles y capturas de pantalla a soporte@correo.com");
            context.Wait(MessageReceived);
        }
        [LuisIntent("ProblemaAcceso")]
        public async Task ProblemaAccesoIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Entiendo que tu problema se debe al acceso del sistema ");
            context.Wait(MessageReceived);
            await context.PostAsync("Te recomiendo que contactes a tu administrador para que resature tu usuario y clave");
        }
        [LuisIntent("Ayuda")]
        public async Task HelpIntent(IDialogContext context, LuisResult result)
        {

            await context.PostAsync("Siento las molestias por el problema, puedes indicarme sii tu problema es alguno de los siguientes:");
            await context.PostAsync("1. No puedes ingresar al sistema");
            await context.PostAsync("2. No puedes ver el curso e-learning");
            await context.PostAsync("3. Otro");
            context.Wait(solution);
        }

        
        private async Task NameReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var username = activity.Text;
            await context.PostAsync($"Que tal {username} te puedo ayudar con alguno de los siguientes problemas?");
            await context.PostAsync("1. No puedes ingresar al sistema");
            await context.PostAsync("2. No puedes ver el curso e-learning");
            await context.PostAsync("3. Otro");
            
            context.Wait(solution);
        }
         private async Task problemReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            
            await context.PostAsync("1. No puedes ingresar al sistema");
            await context.PostAsync("2. No puedes ver el curso e-learning");
            await context.PostAsync("3. Otro");
            
            context.Wait(solution);
        }
        private async Task solution(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var problem = activity.Text;
            await context.PostAsync($"El problema que tiene es el numero {problem}");
            if(problem=="1"||problem=="sistema"){
                await context.PostAsync("Para ingresar al sistema considera las letras  mayusculas, minusculas y retira espacios en blanco");
                
                
            }
            if(problem=="2"||problem=="curso"){
                await context.PostAsync("Para ver el curso asegurate de tener habilitado todos los requerimientos del sitio");
                
                
            }
            if (problem=="3")
            {
                await context.PostAsync("Describe tu problema para que pueda buscar en mis registros una solucion");
            }
            context.Wait(MessageReceived);
        }
        
      
        private async Task ShowLuisResult(IDialogContext context, LuisResult result) 
        {
            await context.PostAsync($"You have reached {result.Intents[0].Intent}. You said: {result.Query}");
            context.Wait(MessageReceived);
        }
    }
}