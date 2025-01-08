# Step 1: Build the Angular app
FROM node:18 AS build

# Set working directory
WORKDIR /app

# Copy package.json and package-lock.json
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the source code
COPY . .

# Build the Angular app with the production configuration
RUN npx ng build leocode-frontend --configuration production

# Step 2: Serve the app using Nginx
FROM nginx:alpine

# Copy the built app from the build stage
COPY --from=build /app/dist/leocode-frontend /usr/share/nginx/html

# Expose the port that Nginx will use
EXPOSE 80

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]
