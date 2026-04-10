-- Created by eric on 4/7/26 - 15:40:46
CREATE TABLE ingredient_provider
(
    ingredient_id INT            NOT NULL,
    provider_id   UUID           NOT NULL,
    price         DECIMAL(19, 4) NOT NULL,
    PRIMARY KEY (ingredient_id, provider_id),
    FOREIGN KEY (ingredient_id) REFERENCES ingredient (id),
    FOREIGN KEY (provider_id) REFERENCES provider (id)
)