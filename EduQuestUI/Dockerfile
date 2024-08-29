
FROM node:18-alpine

# Step 2: Set the working directory inside the container
WORKDIR /app

# Step 3: Copy the package.json and pnpm-lock.yaml to install dependencies
COPY package.json pnpm-lock.yaml ./

# Step 4: Install dependencies using pnpm
RUN npm install -g pnpm && pnpm install

# Step 5: Copy the rest of the application files to the container
COPY . .

# Step 6: Build the application
RUN pnpm run build

CMD ["pnpm", "run", "preview", "--host"]

EXPOSE 4173
