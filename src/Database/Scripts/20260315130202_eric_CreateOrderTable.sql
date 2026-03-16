-- Created by eric on 3/15/26 - 13:02:02
CREATE TABLE bill
(
    id         UUID PRIMARY KEY default (gen_random_uuid()),
    amount     DECIMAL(12, 2) NOT NULL,
    created_at TIMESTAMPTZ      default now(),
    created_by UUID           NOT NULL,
    FOREIGN KEY (created_by) REFERENCES host_user (id)
)