// Switch to the transactions database
db = db.getSiblingDB("transactions");

// Create a new application user (not root) for transactionsDb
db.createUser({
    user: "appuser",
    pwd: "password123",
    roles: [
        {
            role: "readWrite",
            db: "transactions"
        }
    ]
});

// Create a collection to ensure database creation
db.createCollection("transactions");

print("Database and user created successfully.");
