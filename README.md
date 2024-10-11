# DMShop

## Contributors
- **RoedGroed** - Jonas R
- **andersthomsen92** - Anders T
- **Autz3n** - Mads A

---

## Setup

To get started with the DMShop application, follow the steps below:

1. **Start Docker Compose:**
    - Navigate to the `DMShop` directory.
    - Run the following command to start the Docker containers:
      ```bash
      docker-compose up
      ```

2. **Connect to the Database:**
    - Once Docker is running, connect to the database Docker container.

3. **Run Database Script:**
    - Execute the `genDb.sql` script to initialize the database schema. The script is located in the following directory:
      ```
      server/dataaccess/genDb.sql
      ```

4. **Start the Server:**
    - Navigate to the `server/API` directory.
    - Start the server using the command:
      ```bash
      dotnet run
      ```

5. **Install Client Dependencies:**
    - Navigate to the `client` directory.
    - Install the required dependencies by running:
      ```bash
      npm install
      ```

6. **Start the Client:**
    - In the `client` directory, start the client-side development server by running:
      ```bash
      npm run dev
      ```

---

## Notes
- Ensure Docker is installed and running on your machine before proceeding.
- The server runs on .NET, and the client is built using React with TypeScript.
- The setup assumes you have both .NET and Node.js installed globally on your system.
