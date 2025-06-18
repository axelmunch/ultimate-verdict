const fillCreateVoteForm = (): string => {
  cy.visit("/");

  cy.get("[data-create-vote]").click();

  // Vote content
  const voteName = `Vote name ${crypto.randomUUID()}`;
  cy.get("[data-name]").type(voteName);
  cy.get("[data-description]").type("Vote description");

  cy.get("[data-visibility-select]").click();
  cy.get("[data-visibility]").eq(1).click();

  cy.get("[data-type-select]").click();
  cy.get("[data-type]").eq(2).click();

  cy.get("[data-victory-condition-select]").click();
  cy.get("[data-victory-condition]").eq(2).click();

  cy.get("[data-replay-on-draw]").click();

  cy.get("[data-option]").eq(0).type("Option test 1");
  cy.get("[data-option]").eq(1).type("Option test 2");
  cy.get("[data-option-add]").click();
  cy.get("[data-option]").eq(2).type("Option test 3");

  cy.get("[data-winners-by-round-count]").type("{selectall}2");

  cy.get("[data-winners-by-round]").eq(0).type("{selectall}2");

  cy.get("[data-round-duration]").type("{selectall}2");
  cy.get("[data-round-duration-unit-select]").click();
  cy.get("[data-round-duration-unit]").eq(0).click();

  // Submit
  cy.get("[data-submit-create-vote]").click();

  // Check
  cy.get("[data-votes]").contains(voteName);

  return voteName;
};

export default fillCreateVoteForm;
