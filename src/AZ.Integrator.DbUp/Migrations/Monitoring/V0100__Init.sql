START TRANSACTION;

CREATE TABLE IF NOT EXISTS monitoring.event_logs (
    id BIGSERIAL PRIMARY KEY,
    tenant_id VARCHAR(100) NOT NULL,
    event_name VARCHAR(200) NOT NULL,
    event_type VARCHAR(100) NULL,        -- e.g. DomainEvent / IntegrationEvent / SystemEvent
    source_module VARCHAR(100) NOT NULL, -- e.g. Shipments, Invoices, Orders
    reference_id UUID NULL,              -- e.g. ShipmentId, InvoiceId itp.
    reference_number VARCHAR(100) NULL,  -- e.g. ShipmentNumber, InvoiceNumber
    payload JSONB NULL,                  -- event serialized
    created_by_id UUID NOT NULL,
    created_by_name VARCHAR(100) NULL,
    created_at TIMESTAMP with time zone NOT NULL DEFAULT NOW(),
    correlation_id VARCHAR(200) NULL,    -- e.g. request chain
    metadata JSONB NULL                  -- elastic field e.g. { "durationMs": 320, "source": "HangfireJob" }
);

CREATE INDEX idx_event_logs__created_at ON monitoring.event_logs(created_at DESC);
CREATE INDEX idx_event_logs__tenant_id ON monitoring.event_logs(tenant_id);
CREATE INDEX idx_event_logs__event_name ON monitoring.event_logs(event_name);

COMMIT;