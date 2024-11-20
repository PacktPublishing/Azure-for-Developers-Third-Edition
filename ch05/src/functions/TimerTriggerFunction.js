const { app } = require('@azure/functions');

app.timer('TimerTriggerFunction', {
    schedule: '0 */5 * * * *',
    handler: (myTimer, context) => {
        context.log('Timer function processed request.');
    }
});
