import Ranking from "../../src/components/voting_system.tsx/Ranking";
import type { Option } from "../../src/types";

describe("Ranking.cy.tsx", () => {
  it("mounts", () => {
    cy.mount(
      <Ranking options={[]} setCanSubmit={() => {}} setDecisions={() => {}} />,
    );

    cy.get("[data-component]").should("exist");
  });

  it("has right options in the default order", () => {
    cy.fixture("options.json").then((optionsData: Option[]) => {
      const options = optionsData.map((option) => ({
        ...option,
      }));

      cy.mount(
        <Ranking
          options={options}
          setCanSubmit={() => {}}
          setDecisions={() => {}}
        />,
      );

      cy.get("[data-option]")
        .should("have.length", optionsData.length)
        .each((el, index) => {
          expect(el.text()).to.contains(optionsData[index].name);
        });
    });
  });
});
