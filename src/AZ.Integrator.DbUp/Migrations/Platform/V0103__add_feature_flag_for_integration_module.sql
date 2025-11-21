START TRANSACTION;

INSERT INTO platform.feature_flags (code, description) VALUES

    ('integrations-module', 'Enable the integrations module');
    
COMMIT;