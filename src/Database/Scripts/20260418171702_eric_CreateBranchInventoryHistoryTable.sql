-- Created by eric on 4/18/26 - 17:17:02
CREATE TYPE branch_inventory_action AS ENUM ('purchase', 'transfer_reception', 'consumption', 'transfer_send', 'damage');

CREATE TABLE branch_inventory_history
(
    id                SERIAL4 PRIMARY KEY,
    previous_quantity DECIMAL                 NOT NULL,
    new_quantity      DECIMAL                 NOT NULL,
    date              TIMESTAMPTZ             NOT NULL DEFAULT now(),
    action            branch_inventory_action NOT NULL,
    ingredient_id     INT                     NOT NULL,
    branch_id         UUID                    NOT NULL,
    user_id           UUID                    NOT NULL,
    FOREIGN KEY (ingredient_id) REFERENCES ingredient (id),
    FOREIGN KEY (branch_id) REFERENCES branch (id),
    FOREIGN KEY (user_id) REFERENCES host_user (id)
)
