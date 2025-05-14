FROM node:23 
WORKDIR /usr/src/app 
COPY package*.json ./ 
RUN npm install 
COPY . . 

EXPOSE 3000 
CMD [ "node", "sidecar.js" ] 